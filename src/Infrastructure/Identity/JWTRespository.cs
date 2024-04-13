using CSRedis;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WT.DirectLogistics.Application.Common.Interfaces;
using WT.DirectLogistics.Domain.Entities;

namespace WT.DirectLogistics.Infrastructure.Identity
{
    public class JwtRespository : IJwtRespository
    {
        private readonly IdentityConfig _config;
        private readonly CSRedisClient _cache;
        public JwtRespository(IOptions<IdentityConfig> config, CSRedisClient cache)
        {
            _config = config.Value;
            _cache = cache;
        }

        /// <summary>
        /// 通过token获取会员id
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Huiyuan> AttachUserToContext(string token)
        {
            try
            {
                Huiyuan model = new Huiyuan();
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config.Secret);
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out _);
                model.Id = int.Parse(claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                model.Cust_Name = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
                model.Cust_Kind = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == nameof(model.Cust_Kind)).Value;
                model.Verify = GetParam(claimsPrincipal.Claims.FirstOrDefault(x => x.Type == nameof(model.Verify)).Value);
                model.Vlevel = GetParam(claimsPrincipal.Claims.FirstOrDefault(x => x.Type == nameof(model.Vlevel)).Value);
                model.Price = GetParam(claimsPrincipal.Claims.FirstOrDefault(x => x.Type == nameof(model.Price)).Value);
                model.Vyear = GetParam(claimsPrincipal.Claims.FirstOrDefault(x => x.Type == nameof(model.Vyear)).Value);
                model.Vip = GetParam(claimsPrincipal.Claims.FirstOrDefault(x => x.Type == nameof(model.Vyear)).Value) == 1 ? true : false;
                model.Source = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == nameof(model.Source))?.Value+"";
                model.LoginToken = GetParam(claimsPrincipal.Claims.FirstOrDefault(x => x.Type == nameof(model.LoginToken))?.Value);
                //直接校验
                if (await CheckIsSingleLogin(model.Id, model.LoginToken, model.Source, token))
                {
                    return model;
                }
                else {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public int GetParam(object param)
        {
            int index = 0;
            int.TryParse(param + "", out index);
            return index;
        }

        public string GetRedisKey(int cust_id, string source, int state)
        {

            switch (state)
            {
                case 1:
                    return "";
                case 2:
                    return "EXT:wt:loginState:" + cust_id;
                case 3:
                    return "EXT:wt:loginState:" + cust_id + source;
                default:
                    break;
            }
            return "error";
        }

        /// <summary>
        /// 校验登录密码是否存在
        /// </summary>
        /// <param name="cust_id"></param>
        /// <param name="LoginState"></param>
        /// <param name="source"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> CheckIsSingleLogin(int cust_id, int LoginState, string source, string token)
        {
            string rediskey = GetRedisKey(cust_id, source, LoginState);
            if (!string.IsNullOrWhiteSpace(rediskey))
            {
                if (rediskey == "error")
                {
                    return false;
                }
                string value = (await _cache.GetAsync<string>(rediskey)) + "";
                if (value == token)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            return true;
        }
    }
}
