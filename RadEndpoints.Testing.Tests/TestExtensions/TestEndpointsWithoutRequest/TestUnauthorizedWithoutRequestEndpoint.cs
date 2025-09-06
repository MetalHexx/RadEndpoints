namespace RadEndpoints.Testing.Tests
{
    public class TestUnauthorizedWithoutRequestEndpoint : RadEndpointWithoutRequest<TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(CancellationToken ct)
        {
            SendUnauthorized("Unauthorized access");
            return Task.CompletedTask;
        }
    }
}