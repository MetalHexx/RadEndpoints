using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace RadEndpoints
{
    public interface IRadEndpoint
    {
        void Configure();
        void EnableValidation();
        void SetBuilder(IEndpointRouteBuilder routeBuilder);
        void SetContext(IHttpContextAccessor contextAccessor);
        void SetEnvironment(IWebHostEnvironment env);
        void SetLogger(ILogger logger);
        T Service<T>() where T : notnull;
    }
}