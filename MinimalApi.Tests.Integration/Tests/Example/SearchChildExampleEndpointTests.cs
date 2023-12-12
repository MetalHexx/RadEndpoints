using MinimalApi.Features.Examples.GetExampleChild;

namespace MinimalApi.Tests.Integration.Tests.Example
{
    [Collection("Endpoint")]
    public class SearchChildExampleEndpointTests(RadEndpointFixture f)
    {
        [Fact]
        public async Task Given_ParentWithChildExists_ReturnsSuccess()
        {
            //Act
            var r = await f.Client.GetAsync<SearchChildExampleEndpoint, SearchChildExampleRequest, SearchChildExampleResponse>(new()
            {
                ParentId = 4,
                FirstName = "Luke",
                LastName = "Skywalker"
            });

            //Assert
            r.Http.StatusCode.Should().Be(HttpStatusCode.OK);
            r.Content.Should().BeOfType<SearchChildExampleResponse>();            
            r.Content.Data.Should().Contain(e => e.FirstName == "Luke" && e.LastName == "Skywalker");
            r.Content.Message.Should().Be("Children found");
        }

        [Theory]
        [InlineData("", "", 4)]
        [InlineData("Luke", "Skywalker", 0)]
        [InlineData("Luke", "Skywalker", -1)]
        public async Task When_InvalidRequest_ReturnsProblem(string firstName, string lastName, int parentId)
        {
            //Act
            var r = await f.Client.GetAsync<SearchChildExampleEndpoint, SearchChildExampleRequest, ProblemDetails>(new()
            {
                ParentId = parentId,
                FirstName = firstName,
                LastName = lastName
            });
            //Assert
            r.Http.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            r.Content.Should().BeOfType<ProblemDetails>();
            r.Content.Should().NotBeNull();
        }

        [Fact]
        public async Task Given_ParentDoesntExists_ReturnsProblem()
        {
            //Act
            var r = await f.Client.GetAsync<SearchChildExampleEndpoint, SearchChildExampleRequest, ProblemDetails>(new()
            {
                ParentId = 1,
                FirstName = "Luke",
                LastName = "Skywalker"
            });
            //Assert
            r.Http.StatusCode.Should().Be(HttpStatusCode.NotFound);
            r.Content.Should().BeOfType<ProblemDetails>();
            r.Content.Should().NotBeNull();
        }
    }
}