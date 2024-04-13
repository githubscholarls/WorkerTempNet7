using System.Threading.Tasks;
using WT.DirectLogistics.Domain.Entities;

namespace WT.DirectLogistics.Application.Common.Interfaces
{
    public interface ILoginHelper
    {
        /// <summary>
        /// 验证用户是否登录
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        Task<Huiyuan> IsLogin();

        /// <summary>
        /// filter 校验是否登录
        /// </summary>
        /// <returns></returns>
        Task<bool> IsLoginExists();
    }
}
