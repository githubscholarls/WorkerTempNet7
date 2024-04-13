using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WT.DirectLogistics.Application.Common.Models
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
        public string Detail { get; set; }="SUCCESS";
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

    public class Json
    {
        public int Status { get; set; }

        public string Detail { get; set; }

        public object Data { get; set; }
        public object Token { get; set; }

        public static Json Success()
        {
            return new Json
            {
                Status = 1000,
                Detail = "success"
            };
        }
        public static Json SuccessData(object data)
        {
            return new Json
            {
                Status = 1000,
                Detail = "success",
                Data = data
            };
        }
        public static Json SuccessDetail(string data)
        {
            return new Json
            {
                Status = 1000,
                Detail = data
            };
        }
        public static string ToJSONStr(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static Json Failure(int status, string detail)
        {
            return new Json
            {
                Status = status,
                Detail = detail
            };
        }

        /// <summary>
        /// 文案提示
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        public static Json ShowToast(string detail)
        {
            return new Json
            {
                Status = 1403,
                Detail = detail
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="detail"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Json ShowToast(int status, string detail, object data)
        {
            return new Json
            {
                Status = status,
                Detail = detail,
                Data = data
            };
        }
    }
}
