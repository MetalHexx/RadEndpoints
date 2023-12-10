using System.Collections.Concurrent;
using System.Net;
using System.Reflection;
using RadEndpoint = MinimalApi.Http.Endpoints.RadEndpoint;

namespace MinimalApi.Tests.Integration.Common
{
    public static class RouteExtensions
    {
        public static string GetAndMapRoute<TEndpoint, TRequest>(TRequest request)
            where TEndpoint : RadEndpoint
        {
            var route = RadEndpoint.GetRoute<TEndpoint>();
            route = route.MapRoute(request!);
            return route;
        }

        /// <summary>
        /// Speeds up test runs by caching property info and reducing reflection calls
        /// </summary>
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _propertyCache = new();

        public static string MapRoute(this string route, object request)
        {
            foreach (var property in request.GetType().GetProperties())
            {
                var value = property.GetValue(request)
                    ?.ToString()
                    ?.WebEncode();

                if (string.IsNullOrEmpty(value)) continue;

                if(route.Contains(property.Name, StringComparison.OrdinalIgnoreCase))
                {
                    route = route.Replace($"{{{property.Name}}}", value, StringComparison.OrdinalIgnoreCase);
                }
                else
                {   
                    route = route.Contains("?") 
                        ? route + $"&{property.Name}={value}"
                        : route + $"?{property.Name}={value}";
                }
            }
            return route;
        }
        private static string WebEncode(this string value) => WebUtility.UrlEncode(value);
    }
}
