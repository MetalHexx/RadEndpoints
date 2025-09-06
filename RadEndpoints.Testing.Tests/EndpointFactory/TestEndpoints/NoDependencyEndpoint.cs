using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RadEndpoints.Testing.Tests
{
    public class NoDependencyEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        public override void Configure()
        {
            Get("/no-dependency")
                .Produces<TestResponse>(StatusCodes.Status200OK)
                .WithDocument(tag: "NoDependency", desc: "An endpoint with no dependencies.");
        }

        public override Task Handle(TestRequest r, CancellationToken ct)
        {
            Logger.LogInformation("Handling NoDependencyEndpoint with IntProperty: {IntProperty}", r.IntProperty);

            Response = new TestResponse
            {
                IntProperty = r.IntProperty,
                StringProperty = r.StringProperty,
                HeaderValue = HttpContext.Request.Headers["mock-header"].ToString(),
                EnvironmentName = Env?.EnvironmentName!
            };

            Send();
            return Task.CompletedTask;
        }
    }
}
