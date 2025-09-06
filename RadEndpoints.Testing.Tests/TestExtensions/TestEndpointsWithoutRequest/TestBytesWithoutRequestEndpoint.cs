namespace RadEndpoints.Testing.Tests
{
    public class TestBytesWithoutRequestEndpoint : RadEndpointWithoutRequest<TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(CancellationToken ct)
        {
            var bytes = new RadBytes
            {
                Bytes = new byte[] { 1, 2, 3, 4 },
                ContentType = "application/octet-stream"
            };
            SendBytes(bytes);
            return Task.CompletedTask;
        }
    }
}