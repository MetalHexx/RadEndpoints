using System.Reflection;

namespace RadEndpoints.Cli.Helpers
{
    internal static class PathHelper
    {
        public static string GetAssemblyRootedPath(this string path) 
        {
            var assemblyLoc = Assembly.GetExecutingAssembly().Location;
            var assemblyPath = Path.GetDirectoryName(assemblyLoc) ?? string.Empty;
            return Path.Combine(assemblyPath, path);
        }
    }
}
