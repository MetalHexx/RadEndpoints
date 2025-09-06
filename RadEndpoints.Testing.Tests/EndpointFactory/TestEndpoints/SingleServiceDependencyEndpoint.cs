using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RadEndpoints.Testing.Tests
{
    public class SingleServiceDependencyEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        private readonly IServiceDependency _dependency;

        public SingleServiceDependencyEndpoint(IServiceDependency dependency)
        {
            _dependency = dependency;
        }

        public override void Configure()
        {
            Get("/single-service-dependency")
                .Produces<TestResponse>(StatusCodes.Status200OK)
                .WithDocument(tag: "SingleServiceDependency", desc: "An endpoint with a single service dependency.");
        }

        public override Task Handle(TestRequest r, CancellationToken ct)
        {
            Logger.LogInformation("Handling SingleServiceDependencyEndpoint with TestProperty: {TestProperty}", r.IntProperty);

            var intResult = _dependency.GetInt();
            var stringResult = _dependency.GetString();
            var headerValue = HttpContext.Request.Headers["Test-Header"];

            Response = new TestResponse
            {
                IntProperty = r.IntProperty + intResult,
                StringProperty = r.StringProperty +stringResult,
                HeaderValue = headerValue.ToString()
            };

            Send();
            return Task.CompletedTask;
        }
    }
}
