using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SFM.DBLite
{
    public static class ModelExtensions
    {
        public static object[] GetValuesAsArray<T>(this T model)
        {
            return model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite)
                .Select(p => p.GetValue(model))
                .ToArray();
        }

        public static object GetValue<T>(this T model, string key)
        {
            var pInfo = model.GetType().GetProperty(key);
            return pInfo.GetValue(model);
        }

        public static string[] GetKeysAsArray<T>(this T model)
        {
            var array = model.GetType().GetProperties()
                .Where(p => p.CanWrite)
                .Select(p => p.Name)
                .ToArray();
            return array;
        }

        public static void SetValue<T>(this T model, string key, string stringVal)
        {
            try
            {
                var pInfo = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .FirstOrDefault(p => p.CanWrite && p.Name == key);
                if(pInfo.PropertyType == typeof(int))
                    pInfo.SetValue(model, int.Parse(stringVal));
                else
                    pInfo.SetValue(model, stringVal);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
