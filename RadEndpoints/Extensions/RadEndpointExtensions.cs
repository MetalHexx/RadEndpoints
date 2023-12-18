using Microsoft.Extensions.Logging;

namespace RadEndpoints
{
    public static class RadEndpointExtensions
    {
        public static Type GetLoggerType(this Type endpointType) => typeof(ILogger<>).MakeGenericType(endpointType); 
        public static Type? GetMapperType(this IRadEndpoint endpoint) => endpoint.GetGenericTypeArgument(typeof(IRadMapper));

        public static Type? GetRequestType(this IRadEndpoint endpoint) => endpoint.GetGenericTypeArgument(typeof(RadRequest));

        public static Type? GetGenericTypeArgument(this IRadEndpoint endpoint, Type targetType)
        {
            var endpointType = endpoint.GetType();

            while (endpointType != null && endpointType != typeof(object))
            {
                if (endpointType.IsGenericType)
                {
                    var genericArguments = endpointType.GetGenericArguments();
                    var matchingType = genericArguments.FirstOrDefault(arg => targetType.IsAssignableFrom(arg));

                    if (matchingType != null)
                    {
                        return matchingType;
                    }
                }

                endpointType = endpointType.BaseType;
            }

            return null;
        }
    }
}
