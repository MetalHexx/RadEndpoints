namespace RadEndpoints.Testing.Tests
{
    public class TestStreamWithoutRequestEndpoint : RadEndpointWithoutRequest<TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(CancellationToken ct)
        {
            var stream = new MemoryStream("Hello World"u8.ToArray());
            var radStream = new RadStream
            {
                Stream = stream,
                ContentType = "text/plain",
                FileDownloadName = "test.txt"
            };
            SendStream(radStream);
            return Task.CompletedTask;
        }
    }
}