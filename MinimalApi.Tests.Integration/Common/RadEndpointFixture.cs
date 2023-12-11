namespace MinimalApi.Tests.Integration.Common
{
    public class RadEndpointFixture : IDisposable
    {
        public HttpClient Client => _factory.CreateClient(); //creating a new client for each test ensure complete test isolation
        public Fixture DataGenerator { get; private set; } = new();

        private readonly WebApplicationFactory<Program> _factory;
        public RadEndpointFixture()
        {
            _factory = new WebApplicationFactory<Program>();
        }
        public void Dispose() => _factory.Dispose();
    }
}