namespace RadEndpoints.Testing.Tests
{
    public class TestRadProblemWithoutRequestEndpoint : RadEndpointWithoutRequest<TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(CancellationToken ct)
        {
            SendProblem(Problem.NotFound("Resource not found"));
            return Task.CompletedTask;
        }
    }
}