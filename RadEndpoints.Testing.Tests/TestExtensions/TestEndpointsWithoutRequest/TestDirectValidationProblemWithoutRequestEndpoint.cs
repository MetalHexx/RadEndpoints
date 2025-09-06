namespace RadEndpoints.Testing.Tests
{
    public class TestDirectValidationProblemWithoutRequestEndpoint : RadEndpointWithoutRequest<TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(CancellationToken ct)
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