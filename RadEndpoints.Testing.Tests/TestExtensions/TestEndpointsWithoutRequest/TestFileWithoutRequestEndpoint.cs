namespace RadEndpoints.Testing.Tests
{
    public class TestFileWithoutRequestEndpoint : RadEndpointWithoutRequest<TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(CancellationToken ct)
        {
            var file = new RadFile
            {
                Path = "/path/to/file.txt",
                ContentType = "text/plain"
            };
            SendFile(file);
            return Task.CompletedTask;
        }
    }
}