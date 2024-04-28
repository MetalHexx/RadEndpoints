using Microsoft.Extensions.Logging;
using System.Reflection;

namespace RadEndpoints
{
    public static class RadTypeExtensions 
    {        
        public static Type GetLoggerType(this Type type) => typeof(ILogger<>).MakeGenericType(type);

        public static bool IsAssignableToGenericType(this object obj, Type genericType)
        {
            var type = obj.GetType();
            var interfaces = type.GetInterfaces();
            foreach (var iface in interfaces)
            {
                if (iface.IsGenericType && iface.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
            }
            return false;
        }

        public static IEnumerable<Type> FindConcreteImplementationsOf<T>(this Assembly assembly) where T : class
        {
            return assembly
                .GetTypes()
                .Where(t => t.IsClass
                    && !t.IsAbstract
                    && typeof(T).IsAssignableFrom(t));
        }
    }
}
