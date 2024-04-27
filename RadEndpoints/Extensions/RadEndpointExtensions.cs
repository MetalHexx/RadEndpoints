using Microsoft.Extensions.Logging;

namespace RadEndpoints
{
    public static class RadEndpointExtensions
    {
        public static Type GetLoggerType(this Type endpointType) => typeof(ILogger<>).MakeGenericType(endpointType); 
        public static Type? GetMapperType(this IRadEndpoint endpoint) => endpoint.GetGenericTypeArgument(typeof(IRadMapper));

        public static Type? GetRequestType(this IRadEndpoint endpoint) 
        {
            Type endpointType = endpoint.GetType();

            Type? interfaceType = endpointType
                .GetInterfaces()
                .FirstOrDefault(t => t.IsGenericType 
                    && t.GetGenericTypeDefinition() == typeof(IRadEndpoint<,>));

            if (interfaceType is null) return null;

            Type[] typeArguments = interfaceType.GetGenericArguments();

            if (typeArguments.Length > 0)
            {
                return typeArguments[0];
            }
            return null;
        }

        




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
