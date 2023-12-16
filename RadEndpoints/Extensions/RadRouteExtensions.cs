using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;

namespace RadEndpoints
{
    public static class RadRouteExtensions
    {
        public static string GetAndMapRoute<TEndpoint, TRequest>(TRequest request)
            where TEndpoint : RadEndpoint
        {
            var route = RadEndpoint.GetRoute<TEndpoint>();
            route = route.MapRoute(request!);
            return route;
        }

        public static string MapRoute(this string path, object requestObject)
        {
            var properties = requestObject.GetType().GetProperties();
            bool hasAnyRouteOrQueryAttributes = properties.Any(property => property.IsDefined(typeof(FromRouteAttribute)) || property.IsDefined(typeof(FromQueryAttribute)));

            foreach (var property in properties)
            {
                var isFromRoute = property.IsDefined(typeof(FromRouteAttribute));
                var isFromQuery = property.IsDefined(typeof(FromQueryAttribute));
                var hasNoAttributes = !property.IsDefined(typeof(Attribute));

                if (isFromQuery) path = path.AppendQueryParameter(property, requestObject);

                else if (isFromRoute) path = path.AppendRouteParameter(property, requestObject);

                else if (hasNoAttributes) path = property.ProcessNoAttributeProperty(path, requestObject);
            }
            return path;
        }

        private static string ProcessNoAttributeProperty(this PropertyInfo property, string route, object request)
        {
            if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
            {
                return route;
            }
            if (route.Contains(property.Name, StringComparison.OrdinalIgnoreCase))
            {
                return route.AppendRouteParameter(property, request);
            }
            else
            {
                return route.AppendQueryParameter(property, request);
            }
        }

        private static string AppendQueryParameter(this string route, PropertyInfo property, object requestObject)
        {
            var value = property.GetPropertyValue(requestObject);

            if (string.IsNullOrEmpty(value)) return route;

            return route.Contains("?")
                ? route + $"&{property.Name}={value}"
                : route + $"?{property.Name}={value}";
        }

        private static string AppendRouteParameter(this string route, PropertyInfo property, object requestObject)
        {
            var value = property.GetPropertyValue(requestObject);

            if (string.IsNullOrEmpty(value)) return route;

            return route.Replace($"{{{property.Name}}}", value, StringComparison.OrdinalIgnoreCase);
        }

        private static string? GetPropertyValue(this PropertyInfo property, object requestObject) => property.GetValue(requestObject)?.ToString()?.WebEncode();
        private static string WebEncode(this string value) => WebUtility.UrlEncode(value);
    }
}
