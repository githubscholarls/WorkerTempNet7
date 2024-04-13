using System;
using System.Reflection;

namespace WT.Trigger.Application.Common.Common
{
    public class ConvertToObjectHelper
    {
        public static object GetModel(string value, Type t)
        {
            object model = Activator.CreateInstance(t);
            PropertyInfo[] prop = model.GetType().GetProperties();
            foreach (var item in prop)
            {
                if (item.PropertyType.ToString().ToLower() == "system.string")
                {
                    item.SetValue(model, value, null);
                    break;
                }
            }
            return model;
        }
    }
}
