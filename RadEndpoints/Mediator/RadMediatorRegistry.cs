using Microsoft.Extensions.DependencyInjection;

namespace RadEndpoints.Mediator
{

    internal class RadMediatorRegistry(IServiceProvider _serviceProvider) : IRadMediatorRegistry
    {
        private static readonly Dictionary<Type, RadMediatorRegistration> _registrations = [];

        public RadMediatorRegistration GetRegistration(Type endpointType)
        {
            if (!_registrations.TryGetValue(endpointType, out var registration))
            {
                throw new InvalidOperationException($"No endpoint found for request type {endpointType.Name}.");
            }
            return registration;
        }

        public void RegisterEndpoints()
        {
            if (_registrations.Count > 0) return;

            var endpoints = GetScopedEndpoints();

            foreach (var endpoint in endpoints)
            {
                TryRegisteringEndpoint(endpoint);
            }
        }

        private static void TryRegisteringEndpoint(IRadEndpoint endpoint)
        {
            var endpointType = endpoint.GetType();

            var alreadyExists = _registrations.TryGetValue(endpointType, out var _);

            if (alreadyExists)
            {
                throw new InvalidOperationException($"Endpoint with request or response type {endpointType.Name} is already registered. Ensure your endpoints have a unique request and response model.");
            }
            _registrations.TryAdd(endpointType, new RadMediatorRegistration
            {
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