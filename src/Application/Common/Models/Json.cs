using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WT.Trigger.Application.Common.Models
{
    public class Json<T>
    {
        /// <summary>
        /// 1000:成功，非100失败
        /// </summary>
        public int Status { get; set; } = 1000;

        public T Data { get; set; }

        /// <summary>
        /// 状态描述
        /// </summary>
        public string Detail { get; set; }="success";
    }

    public class JsonResult 
    {
        public int Status { get; set; }

        public string Detail { get; set; }

        public static JsonResult Success()
        {
            return new JsonResult
            {
                Status = 1000,
                Detail = "success"
            };
        }

        public static JsonResult Failure(int status, string detail)
        {
            return new JsonResult
            {
                Status = status,
                Detail = detail
            };
        }
    }
}
