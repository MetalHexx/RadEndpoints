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
                var value = property.GetValue(request);

                if (value is null) continue;

                route = route.Replace($"{{{property.Name}}}", WebUtility.UrlEncode(value.ToString()), StringComparison.OrdinalIgnoreCase);
            }
            return route;
        }
    }
}
