using System;
using Newtonsoft.Json;

namespace WT.Trigger.Application.Common.Common
{
    public class JsonHelper
    {
        public static bool IsJson(string value, Type type)
        {
            try
            {
                JsonConvert.DeserializeObject(value, type);

                return true;
            }
            catch
            {

                return false;
            }
        }
    }
}
