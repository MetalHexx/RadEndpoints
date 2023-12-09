using MinimalApi.Features.Examples.GetExample;

namespace MinimalApi.Tests.Integration.Tests.Example
{
    [Collection("Endpoint")]
    public class GetExampleEndpointTests
    {
        private readonly EndpointFixture _fixture;
        public GetExampleEndpointTests(EndpointFixture fix) => _fixture = fix;

        [Fact]
        public async void When_GetExampleEndpoint_Called_Returns_Success()
        {
            //Act            
            var (h, r) = await _fixture.Client.GetAsync<GetExampleEndpoint, GetExampleRequest, GetExampleResponse>(new()
            {
                Id = 1
            });

            //Assert
            h.StatusCode.Should().Be(HttpStatusCode.OK);
            r.Should().NotBeNull();
            r!.Data!.Id.Should().Be(1);
        }

        [Fact]
        public async void When_GetExampleEndpoint_Called_With_Invalid_Id_Returns_NotFound()
        {
            //Act            
            var (h, r) = await _fixture.Client.GetAsync<GetExampleEndpoint, GetExampleRequest, ProblemDetails>(new()
            {
                Id = 999
            });

            //Assert
            h.StatusCode.Should().Be(HttpStatusCode.NotFound);
            r.Should().NotBeNull();
            r.Should().BeOfType<ProblemDetails>();
            r!.Title.Should().Be("Example not found");
            r.Status.Should().Be(StatusCodes.Status404NotFound);
        }
        [Fact]
        public async void When_GetExampleEndpoint_Called_With_Invalid_Id_Returns_ValidationError()
        {
            //Act            
            var (h, r) = await _fixture.Client.GetAsync<GetExampleEndpoint, GetExampleRequest, ProblemDetails>(new()
            {
                Id = 0
            });

            //Assert
            h.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            r.Should().NotBeNull();
            r.Should().BeOfType<ProblemDetails>();
            r!.Title.Should().Be("Validation Error");
            r.Status.Should().Be(StatusCodes.Status400BadRequest);
        }
    }
}
