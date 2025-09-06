namespace RadEndpoints.Testing.Tests
{
    public class TestDirectProblemHttpResultEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(TestRequest r, CancellationToken ct)
        {
            var problemResult = Microsoft.AspNetCore.Http.TypedResults.Problem("I'm a teapot", statusCode: 418);
            SendProblem(problemResult);
            return Task.CompletedTask;
        }
    }
}