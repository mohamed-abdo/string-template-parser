using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StringTemplateParser
{
    public static class HelperExt
    {
        public static IDictionary<string, object> ToDictionary(this object source)
        {
            return source.GetType().GetProperties(
                BindingFlags.DeclaredOnly |
                BindingFlags.Public |
                BindingFlags.Instance).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null) ?? string.Empty
            );
        }
        public static bool IsAnonymousType(this object instance)
        {

            if (instance == null)
                return false;

            return instance.GetType().Namespace == null;
        }
        public static IDictionary<string, object> ToFlattenDictionary(this object source, string parentPropertyKey = null, IDictionary<string, object> parentPropertyValue = null)
        {
            var propsDic = parentPropertyValue ?? new Dictionary<string, object>();
            foreach (var item in source.ToDictionary())
            {
                var key = string.IsNullOrEmpty(parentPropertyKey) ? item.Key : $"{parentPropertyKey}.{item.Key}";
                if (item.Value.IsAnonymousType())
                    return item.Value.ToFlattenDictionary(key, propsDic);
                else
                    propsDic.Add(key, item.Value);
            }
            return propsDic;
        }
    }
}
