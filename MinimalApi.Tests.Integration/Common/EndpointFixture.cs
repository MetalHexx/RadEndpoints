using AutoFixture;

namespace MinimalApi.Tests.Integration.Common
{
    public class EndpointFixture : IDisposable
    {
        public HttpClient Client => _factory.CreateClient();
        public Fixture DataGenerator => new();

        private readonly WebApplicationFactory<Program> _factory;
        public EndpointFixture() => _factory = new WebApplicationFactory<Program>();
        public void Dispose() => _factory.Dispose();
    }
}