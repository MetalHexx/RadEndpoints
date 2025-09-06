namespace RadEndpoints.Testing.Tests
{
    public class TestNotFoundWithoutRequestEndpoint : RadEndpointWithoutRequest<TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(CancellationToken ct)
        {
            SendNotFound("Resource not found");
            return Task.CompletedTask;
        }
    }
}