using Microsoft.AspNetCore.Mvc.Testing;

namespace MinimalApi.Tests.Integration.Common
{
    public class EndpointFixture : IClassFixture<WebApplicationFactory<Program>>
    {
        public HttpClient Client => _factory.CreateClient();

        private readonly WebApplicationFactory<Program> _factory;

        public EndpointFixture(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }
    }
}