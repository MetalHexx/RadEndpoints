using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace RadEndpoints.Mediator
{
    public class RadMediator : IRadMediator
    {
        private static readonly Dictionary<Type, RadMediatorRegistration> _registrations = [];
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;

        public RadMediator(IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
        {
            _serviceProvider = serviceProvider;
            _httpContextAccessor = httpContextAccessor;
            _env = env;
            RegisterEndpoints();
        }

        private void RegisterEndpoints()
        {
            if(_registrations.Count > 0) return;

            var endpoints = GetScopedEndpoints();

            foreach (var endpoint in endpoints)
            {
                Type? endpointKey = GetEndpointKey(endpoint);

                if (endpointKey is null) continue;                

                TryRegisteringEndpoint(endpoint, endpointKey);
            }
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
        
        private static Type? GetEndpointKey(IRadEndpoint endpoint)
        {
            var hasRequest = !endpoint.IsAssignableToGenericType(typeof(IRadEndpointWithoutRequest<>));

            var endpointKey = hasRequest
                ? endpoint.GetRequestType()
                : endpoint.GetResponseType();

            return endpointKey;
        }

        public Task CallHandlerAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
            where TRequest : class
            where TResponse : new()
        {
            var registration = GetRegistration<TRequest>();
            var endpoint = GetEndpointAs<IRadEndpoint<TRequest, TResponse>>(registration.EndpointType);
            ConfigureEndpoint(registration, endpoint);
            return endpoint.Handle(request, cancellationToken);
        }

        public Task CallHandlerAsync<TResponse>(CancellationToken cancellationToken) 
            where TResponse : new()
        {
            var registration = GetRegistration<TResponse>();
            var endpoint = GetEndpointAs<IRadEndpointWithoutRequest<TResponse>>(registration.EndpointType);
            ConfigureEndpoint(registration, endpoint);
            return endpoint.Handle(cancellationToken);
        }

        private void ConfigureEndpoint(RadMediatorRegistration registration, IRadEndpoint endpoint)
        {
            endpoint.SetLogger(_serviceProvider.GetLogger(endpoint.GetType()));
            endpoint.SetContext(_httpContextAccessor);
            endpoint.SetEnvironment(_env);

            if (registration.MapperType == null) return;

            var mapper = _serviceProvider.GetService(registration.MapperType) as IRadMapper
                ?? throw new InvalidOperationException($"Mapper for {registration.MapperType.Name} not found.");

            var endpointWithMapper = endpoint as IRadEndpointWithMapper
                ?? throw new InvalidOperationException($"Endpoint for {endpoint.GetType().Name} does not support mappers.");

            endpointWithMapper.SetMapper(mapper);
        }

        private T GetEndpointAs<T>(Type endpointType) where T: class
        {
            return _serviceProvider.GetService(endpointType) as T
                ?? throw new InvalidOperationException($"Endpoint for {endpointType.Name} not found.");
        }

        private RadMediatorRegistration GetRegistration<T>() 
        {
            if (!_registrations.TryGetValue(typeof(T), out var registration))
            {
                throw new InvalidOperationException($"No endpoint found for request type {typeof(T).Name}.");
            }
            return registration;
        }
    }
}