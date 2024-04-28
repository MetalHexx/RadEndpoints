using Microsoft.Extensions.DependencyInjection;

namespace RadEndpoints.Mediator
{
    public interface IRadMediatorRegistry
    {
        RadMediatorRegistration GetRegistration<T>();
        void RegisterEndpoints();
    }

    public class RadMediatorRegistry(IServiceProvider _serviceProvider) : IRadMediatorRegistry
    {
        private static readonly Dictionary<Type, RadMediatorRegistration> _registrations = [];

        public RadMediatorRegistration GetRegistration<T>()
        {
            if (!_registrations.TryGetValue(typeof(T), out var registration))
            {
                throw new InvalidOperationException($"No endpoint found for request type {typeof(T).Name}.");
            }
            return registration;
        }

        public void RegisterEndpoints()
        {
            if (_registrations.Count > 0) return;

            var endpoints = GetScopedEndpoints();

            foreach (var endpoint in endpoints)
            {
                Type? endpointKey = GetEndpointKey(endpoint);

                if (endpointKey is null) continue;

                TryRegisteringEndpoint(endpoint, endpointKey);
            }
        }

        private static Type? GetEndpointKey(IRadEndpoint endpoint)
        {
            var hasRequest = !endpoint.IsAssignableToGenericType(typeof(IRadEndpointWithoutRequest<>));

            var endpointKey = hasRequest
                ? endpoint.GetRequestType()
                : endpoint.GetResponseType();

            return endpointKey;
        }

        private static void TryRegisteringEndpoint(IRadEndpoint endpoint, Type endpointKey)
        {
            var endpointType = endpoint.GetType();

            var alreadyExists = _registrations.TryGetValue(endpointKey, out var _);

            if (alreadyExists)
            {
                throw new InvalidOperationException($"Endpoint with request or response type {endpointKey.Name} is already registered. Ensure your endpoints have a unique request and response model.");
            }
            _registrations.TryAdd(endpointKey, new RadMediatorRegistration
            {
                EndpointType = endpointType,
                EndpointKey = endpointKey,
                MapperType = endpoint.GetMapperType(),
                LoggerType = endpointType.GetLoggerType()
            });
        }

        private IEnumerable<IRadEndpoint> GetScopedEndpoints()
        {
            using var scope = _serviceProvider.CreateScope();

            var provider = scope.ServiceProvider;

            return provider.GetServices<IRadEndpoint>();
        }

    }
}
