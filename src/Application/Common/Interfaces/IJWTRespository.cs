using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WT.DirectLogistics.Domain.Entities;

namespace WT.DirectLogistics.Application.Common.Interfaces
{
    //创建接口
    public interface IJwtRespository
    {
        /// <summary>
        /// 通过token获取会员id
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Huiyuan> AttachUserToContext(string token);
    }
}
