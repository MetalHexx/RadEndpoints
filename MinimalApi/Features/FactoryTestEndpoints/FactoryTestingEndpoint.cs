
namespace MinimalApi.Features.FactoryTestEndpoints
{
    public class FactoryTestingEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        public override void Configure()
        {
            Get("/factory-test")
                .Produces<TestResponse>(StatusCodes.Status200OK)
                .WithDocument(tag: "FactoryTest", desc: "Test endpoint for factory creation and unit testing with a request and response model.");
        }

        public override Task Handle(TestRequest r, CancellationToken ct)
        {
            Response = new TestResponse { TestProperty = r.TestProperty + 1 };
            Logger.LogDebug("HttpVerb:{HttpVerb}", HttpContext.Request.Method);
            Logger.LogInformation("TestProperty:{TestProperty}", r.TestProperty);
            Logger.LogCritical("AspNetEnvironment:{EnvironmentName}", Env.EnvironmentName);
            Send();
            return Task.CompletedTask;
        }
    }
}
