using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace Leagueinator.Utility.Seek {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class DoSeek : Attribute { }

    public static class SeekAlogrithm {
        /// <summary>
        /// Build a list all member varialbes that are instances of type T that are annotated with 
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static List<T> SeekDeep<T>(this object target) where T : class {
            var list = new List<T>();
            Type type = target.GetType();

            if (target is null) return list;

            // If target is enumerable recurse over all contents
            if (target.GetType().GetInterfaces().Contains(typeof(IEnumerable))) {
                foreach (object? item in (IEnumerable)target) {
                    list.AddRange(item.SeekDeep<T>());
                }
                return list;
            }
            
            // If target is of type, add target to the result
            if (target.GetType() == typeof(T)) {
                if (list.Contains(target)) return list;
                list.Add((T)target);
            }

            // Recurse over each property marked with the "SeekDeep" annotation.
            foreach (PropertyInfo prop in type.GetProperties()) {
                if (prop.GetCustomAttribute<DoSeek>() is null) continue;
                List<T> l = target.SeekDeepHelper<T>(prop);
                list.AddRange(l);
            }

            return list;
        }

        private static List<T> SeekDeepHelper<T>(this object isModel, PropertyInfo prop) where T : class {
            var list = new List<T>();

            if (typeof(IEnumerable<T>).IsAssignableFrom(prop.PropertyType)) {
                // Enumberable of type - ie List or Array
                object? value = prop.GetValue(isModel, null);
                if (value != null) {
                    var values = (IEnumerable<T>)value;
                    if (values != null) list.AddRange(values);
                }
            }
            else if (prop.PropertyType == typeof(T)) {
                // Is of exact type
                var value = (T?)prop.GetValue(isModel, null);
                if (value != null) list.Add(value);
            }
            else if (prop.PropertyType.GetInterfaces().Contains(typeof(IEnumerable))) {
                // Enumerable of not-type
                var value = (IEnumerable?)prop.GetValue(isModel, null);
                if (value != null) {
                    foreach (object? item in value) {
                        list.AddRange(item.SeekDeep<T>());
                    }
                }
            }
            else if (!prop.PropertyType.IsPrimitive) {
                // Some other (non-enumerable) type
                object? value = prop.GetValue(isModel, null);
                if (value != null) list.AddRange(value.SeekDeep<T>());
            }

            return list;
        }
    }
}
