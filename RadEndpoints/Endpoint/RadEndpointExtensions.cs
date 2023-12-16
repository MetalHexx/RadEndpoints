using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace RadEndpoints
{
    public static class RadEndpointExtensions
    {
        public static RouteHandlerBuilder WithDocument(this RouteHandlerBuilder routeBuilder, string tag, string desc) => routeBuilder
            .WithTags(tag)
            .WithDescription(desc)
            .WithOpenApi();
    }
}
