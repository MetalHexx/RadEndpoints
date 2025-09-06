namespace RadEndpoints.Testing.Tests
{
    public class TestAuthenticationChallengeEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(TestRequest r, CancellationToken ct)
        {
            SendUnauthorized();
            return Task.CompletedTask;
        }
    }
}