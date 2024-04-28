using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace RadEndpoints.Mediator
{
    public class RadMediator : IRadMediator
    {        
        private readonly IServiceProvider _serviceProvider;
        private readonly IRadMediatorRegistry _registry;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;

        public RadMediator(IRadMediatorRegistry registry, IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
        {
            _registry = registry;
            _serviceProvider = serviceProvider;
            _httpContextAccessor = httpContextAccessor;
            _env = env;            
            _registry.RegisterEndpoints();
        }

        public Task CallHandlerAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
            where TRequest : class
            where TResponse : new()
        {
            var registration = _registry.GetRegistration<TRequest>();
            var endpoint = GetEndpointAs<IRadEndpoint<TRequest, TResponse>>(registration.EndpointType);
            ConfigureEndpoint(registration, endpoint);
            return endpoint.Handle(request, cancellationToken);
        }

        public Task CallHandlerAsync<TResponse>(CancellationToken cancellationToken) 
            where TResponse : new()
        {
            var registration = _registry.GetRegistration<TResponse>();
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
    }
}