namespace MinimalApi.Tests.Integration.Common
{
    public class RadEndpointFixture : IDisposable
    {
        public HttpClient Client => _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        public Fixture DataGenerator { get; private set; } = new();

        private readonly WebApplicationFactory<Program> _factory;

        public RadEndpointFixture()
        {
            _factory = new WebApplicationFactory<Program>();
        }

        public void Dispose() => _factory.Dispose();
    }
}