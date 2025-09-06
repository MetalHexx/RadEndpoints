namespace RadEndpoints.Testing.Tests
{
    public class TestForbiddenEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(TestRequest r, CancellationToken ct)
        {
            SendForbidden("Forbidden access");
            return Task.CompletedTask;
        }
    }
}