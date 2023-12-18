using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RadEndpoints
{
    public static class RadServiceProviderExtensions
    {
        public static ILogger GetLogger(this IServiceProvider serviceProvider, Type endpointType)
        {
            var loggerType = endpointType.GetLoggerType();
            var logger = (ILogger)serviceProvider.GetRequiredService(loggerType);
            return logger;
        }
        public static bool IsValidatorRegistered(this IServiceProvider serviceProvider, IRadEndpoint endpoint)
        {
            var requestType = endpoint.GetRequestType();

            if (requestType is null) return false;

            var validatorType = typeof(IValidator<>).MakeGenericType(requestType);
            var services = serviceProvider.GetServices(validatorType);

            foreach (var service in services)
            {
                if (service is null) continue;

                if (validatorType.IsAssignableFrom(service.GetType()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
