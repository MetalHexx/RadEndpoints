using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace RadEndpoints.Mediator
{
    public class RadMediator : IRadMediator
    {
        private static readonly ConcurrentDictionary<Type, RadMediatorRegistration> _registrations = new();
        private readonly IServiceProvider _provider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;

        public RadMediator(IServiceProvider serviceProvider)
        {
            _provider = serviceProvider;
            _httpContextAccessor = _provider.GetRequiredService<IHttpContextAccessor>();
            _env = _provider.GetRequiredService<IWebHostEnvironment>();
            RegisterEndpoints();
        }

        private void RegisterEndpoints()
        {
            using var scope = _provider.CreateScope();
            var provider = scope.ServiceProvider;

            var endpoints = provider.GetServices<IRadEndpoint>();

            foreach (var endpoint in endpoints)
            {
                var requestType = endpoint.GetRequestType();
                var mappertype = endpoint.GetMapperType();

                if (requestType is null) continue;

                var endpointType = endpoint.GetType();

                _registrations.TryAdd(requestType, new RadMediatorRegistration
                {
                    EndpointType = endpointType,
                    RequestType = requestType,
                    MapperType = mappertype,
                    LoggerType = endpointType.GetLoggerType()
                });
            }
        }

        public async Task<IResult> SendAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
            where TRequest : RadRequest
            where TResponse : RadResponse, new()
        {
            if (!_registrations.TryGetValue(typeof(TRequest), out var registration))
            {
                throw new InvalidOperationException($"No endpoint found for request type {typeof(TRequest).Name}.");
            }
            var endpoint = _provider.GetService(registration.EndpointType) as RadEndpoint<TRequest, TResponse>
                ?? throw new InvalidOperationException($"Endpoint for {typeof(TRequest).Name} not found.");

            if (registration.MapperType is not null)
            {
                var mapper = _provider.GetService(registration.MapperType) as IRadMapper
                    ?? throw new InvalidOperationException($"Mapper for {registration.MapperType.Name} not found.");

                var endpointWithMapper = endpoint as IRadEndpointWithMapper
                    ?? throw new InvalidOperationException($"Endpoint for {typeof(TRequest).Name} does not support mappers.");

                endpointWithMapper.SetMapper(mapper);
            }
            var e = endpoint as IRadEndpoint;
            e.SetLogger(_provider.GetLogger(endpoint.GetType()));
            e.SetContext(_httpContextAccessor);
            e.SetEnvironment(_env);

            endpoint.Response = new TResponse();
            return await endpoint.Handle(request, cancellationToken);
        }
    }
}