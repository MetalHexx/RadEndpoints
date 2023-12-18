using System.Reflection;

namespace RadEndpoints
{
    public static class RadAssemblyExtensions
    {
        public static IEnumerable<Type> FindConcreteImplementationsOf<T>(this Assembly assembly) where T : class => assembly.GetTypes()
                .Where(t => t.IsClass
                    && !t.IsAbstract
                    && typeof(T).IsAssignableFrom(t));
    }
}
