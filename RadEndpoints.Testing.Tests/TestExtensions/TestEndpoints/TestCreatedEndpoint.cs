namespace RadEndpoints.Testing.Tests
{
    public class TestCreatedEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(TestRequest r, CancellationToken ct)
        {
            var response = new TestResponse { IntProperty = 100 };
            SendCreatedAt("/test/100", response);
            return Task.CompletedTask;
        }
    }
}