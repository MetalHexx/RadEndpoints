using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RadEndpoints.Testing.Tests
{
    public class ServiceAndAnotherDependencyEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        private readonly IServiceDependency _dependency;
        private readonly IAnotherDependency _anotherDependency;

        public ServiceAndAnotherDependencyEndpoint(IServiceDependency dependency, IAnotherDependency anotherDependency)
        {
            _dependency = dependency;
            _anotherDependency = anotherDependency;
        }

        public override void Configure()
        {
            Get("/service-and-another-dependency")
                .Produces<TestResponse>(StatusCodes.Status200OK)
                .WithDocument(tag: "ServiceAndAnotherDependency", desc: "An endpoint with service and another dependency.");
        }

        public override Task Handle(TestRequest r, CancellationToken ct)
        {
            Logger.LogInformation("Handling ServiceAndAnotherDependencyEndpoint with IntProperty: {IntProperty}", r.IntProperty);

            var intResult = _dependency.GetInt();
            var stringResult = _dependency.GetString();
            var anotherResult = _anotherDependency.GetAnotherValue();

            Response = new TestResponse
            {
                IntProperty = r.IntProperty + intResult,
                StringProperty = r.StringProperty + stringResult + anotherResult,
                HeaderValue = HttpContext.Request.Headers["mock-header"].ToString(),
            };

            Send();
            return Task.CompletedTask;
        }
    }
}
