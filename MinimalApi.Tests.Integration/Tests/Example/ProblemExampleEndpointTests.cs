using MinimalApi.Features.Examples.ProblemExample;

namespace MinimalApi.Tests.Integration.Tests.Example
{
    [Collection("Endpoint")]
    public class ProblemExampleEndpointTests(RadEndpointFixture f)
    {
        [Fact]
        public async Task Returns_Problem()
        {
            //Act
            var r = await f.Client.GetAsync<ProblemExampleEndpoint, ProblemExampleRequest, ProblemDetails>(new ProblemExampleRequest());

            //Assert
            r.Should().BeProblem()
                .WithMessage("Problem Example")
                .WithStatusCode(HttpStatusCode.BadRequest)
                .WithContentType("https://example.com/problem")
                .WithDetail("This is an example of a problem details response.")
                .WithInstance("/examples/problem")
                .WithKeyAndValue("example", "This is an example of a problem details response.")
                .WithKeyAndValue("example2", "This is an example of a problem details response.");
        }

        [Fact]
        public async Task Returns_Problem_WithoutRequest()
        {
            //Act
            var r = await f.Client.GetAsync<ProblemExampleEndpoint, ProblemDetails>();

            //Assert
            r.Should().BeProblem()
                .WithMessage("Problem Example")
                .WithStatusCode(HttpStatusCode.BadRequest)
                .WithContentType("https://example.com/problem")
                .WithDetail("This is an example of a problem details response.")
                .WithInstance("/examples/problem")
                .WithKeyAndValue("example", "This is an example of a problem details response.")
                .WithKeyAndValue("example2", "This is an example of a problem details response.");
        }
    }
}
