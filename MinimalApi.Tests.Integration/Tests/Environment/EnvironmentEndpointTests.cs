using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MinimalApi.Features.Examples.GetExample;
using MinimalApi.Tests.Integration.Common;
using System.Net;

namespace MinimalApi.Tests.Integration.Tests.Environment
{
    public class EnvironmentEndpointTests : EndpointFixture
    {
        public EnvironmentEndpointTests(WebApplicationFactory<Program> f) : base(f) { }

        [Fact]
        public async void When_EnvironmentEndpointCalled_Returns_Success()
        {
            // Act            
            var (h, r) = await Client.GetAsync<GetExampleEndpoint, GetExampleRequest, GetExampleResponse>(new()
            {
                Id = 1
            });

            // Assert
            h.StatusCode.Should().Be(HttpStatusCode.OK);
            r.Should().NotBeNull();
            r!.Data!.Id.Should().Be(1);
        }
    }
}