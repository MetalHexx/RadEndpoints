namespace RadEndpoints.Testing.Tests
{
    public class TestRadProblemEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(TestRequest r, CancellationToken ct)
        {
            SendProblem(Problem.NotFound("Resource not found"));
            return Task.CompletedTask;
        }
    }
}