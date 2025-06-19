using System.Reflection;

namespace Scaffold.Core.Extensions
{
    internal static class CopyToExtension
    {
        internal static void CopyTo<T>(this T from, T to)
        {
            Type t = typeof(T);
            PropertyInfo[] props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in props)
            {
                if (!p.CanRead || !p.CanWrite) continue;

                object val = p.GetGetMethod().Invoke(from, null);
                object defaultVal = p.PropertyType.IsValueType ? Activator.CreateInstance(p.PropertyType) : null;
                if (null != val && !val.Equals(defaultVal))
                {
                    p.GetSetMethod().Invoke(to, new[] { val });
                }
            }
        }
    }
}
