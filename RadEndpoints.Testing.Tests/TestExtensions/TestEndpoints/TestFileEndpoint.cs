namespace RadEndpoints.Testing.Tests
{
    public class TestFileEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(TestRequest r, CancellationToken ct)
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