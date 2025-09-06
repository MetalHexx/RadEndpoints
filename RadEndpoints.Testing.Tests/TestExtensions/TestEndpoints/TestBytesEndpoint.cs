namespace RadEndpoints.Testing.Tests
{
    public class TestBytesEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(TestRequest r, CancellationToken ct)
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