namespace RadEndpoints.Testing.Tests
{
    public class TestDirectValidationProblemEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(TestRequest r, CancellationToken ct)
        {
            var errors = new Dictionary<string, string[]>
            {
                { "TestField", new string[] { "Test validation error" } }
            };
            var validationProblem = Microsoft.AspNetCore.Http.TypedResults.ValidationProblem(errors, title: "Validation Error");
            SendProblem(validationProblem);
            return Task.CompletedTask;
        }
    }
}