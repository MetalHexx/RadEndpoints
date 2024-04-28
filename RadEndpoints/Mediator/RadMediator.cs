using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RadEndpoints.Mediator
{
    internal class RadMediator : IRadMediator
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

        public Task CallHandler<TRequest, TResponse>(Type endpointType, TRequest request, CancellationToken cancellationToken)
            where TRequest : class
            where TResponse : new()
        {
            var registration = _registry.GetRegistration(endpointType);
            var endpoint = _serviceProvider.GetService(endpointType) as IRadEndpoint<TRequest, TResponse>;

            if (endpoint is null)
            {
                throw new InvalidOperationException($"Endpoint for {endpointType.Name} not found.");
            }
            ConfigureEndpoint(registration, endpoint);
            return endpoint.Handle(request, cancellationToken);
        }

        public Task CallHandler<TResponse>(Type endpointType, CancellationToken cancellationToken) 
            where TResponse : new()
        {
            var registration = _registry.GetRegistration(endpointType);
            var endpoint = _serviceProvider.GetService(endpointType) as IRadEndpointWithoutRequest<TResponse>;

            if (endpoint is null)
            {
                throw new InvalidOperationException($"Endpoint for {endpointType.Name} not found.");
            }
            ConfigureEndpoint(registration, endpoint);
            return endpoint.Handle(cancellationToken);
        }

        private void ConfigureEndpoint(RadMediatorRegistration registration, IRadEndpoint endpoint)
        {
            var logger = (ILogger)_serviceProvider.GetRequiredService(registration.LoggerType);
            endpoint.SetLogger(logger);
            endpoint.SetContext(_httpContextAccessor);
            endpoint.SetEnvironment(_env);

            if (registration.MapperType == null) return;

            var mapper = _serviceProvider.GetService(registration.MapperType) as IRadMapper
                ?? throw new InvalidOperationException($"Mapper for {registration.MapperType.Name} not found.");

            var endpointWithMapper = endpoint as IRadEndpointWithMapper
                ?? throw new InvalidOperationException($"Endpoint for {endpoint.GetType().Name} does not support mappers.");

            endpointWithMapper.SetMapper(mapper);
        }
    }
}