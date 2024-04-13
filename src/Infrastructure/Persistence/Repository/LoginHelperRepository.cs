using System;
using Dapper;
using System.Text;
using System.Linq;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using WT.DirectLogistics.Domain.Entities;
using WT.DirectLogistics.Application.Common;
using WT.DirectLogistics.Application.Common.Interfaces;
using WT.DirectLogistics.Infrastructure.Identity;
using WT.DirectLogistics.Domain.Common;
using System.Web;

namespace WT.DirectLogistics.Infrastructure.Persistence.Repository
{
    public class LoginHelperRepository : ILoginHelper
    {
        private readonly IDbConnection _dbConnection;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IJwtRespository _jwtrepository;
        public LoginHelperRepository(IDbConnection dbConnection, IHttpContextAccessor httpContext, IJwtRespository jwtrepository)
        {
            _dbConnection = dbConnection;
            _httpContext = httpContext;
            _jwtrepository = jwtrepository;
        }

        /// <summary>
        /// 验证登录数据是否正常
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        public async Task<Huiyuan> IsLogin()
        { 
            string token = _httpContext.HttpContext.Request.Headers["token"];
            token = string.IsNullOrWhiteSpace(token) ? _httpContext.HttpContext.Request.Headers["accesstoken"] : token;
            token = string.IsNullOrWhiteSpace(token) ? _httpContext.HttpContext.Request.Cookies["accesstoken"] : token;
            string userinfo = _httpContext.HttpContext.Request.Cookies["UserInfo"];
            if (!string.IsNullOrWhiteSpace(token))
            {
                if (token.ToLower().StartsWith("bearer ")&& token.Split(' ').Length==2)
                {
                    token = token.Split(' ')[1];
                }
                //使用jwt 解密登录信息
                return await _jwtrepository.AttachUserToContext(token);
            }
            else if (!string.IsNullOrWhiteSpace(userinfo))
            {
                //cookie不为空，验证cookie
                #region 同步会员登录解密方法
                string user = string.Empty;
                if (userinfo.Contains("%"))
                {
                    user = new AES().Decrypto(HttpUtility.UrlDecode(userinfo));//解密
                }
                else
                {                    
                    try
                    {
                        user = DesEncrypt.Decrypt(userinfo);
                    }
                    catch (Exception ex)
                    {
                        user = new AES().Decrypto(userinfo);
                    }
                } 
                #endregion

                int index = user.IndexOf("|");
                //未得到有效登录信息处理，避免异常
                if (index == -1) 
                {
                    return null;
                }
                string cust_name = user.Substring(0, index);
                string cust_pass = user.Substring(index + 1).Replace("CRMAUTO", "");
                return await _dbConnection.QueryFirstOrDefaultAsync<Huiyuan>("select id,cust_name,cust_kind,vip,price from huiyuan where cust_name=@cust_name and cust_pass=@cust_pass;", new
                {
                    cust_name,
                    cust_pass
                });
            }
            return null;
        }

        public async Task<bool> IsLoginExists()
        {
            var model = await IsLogin();
            if (model != null && model.Id > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }
    }
}
