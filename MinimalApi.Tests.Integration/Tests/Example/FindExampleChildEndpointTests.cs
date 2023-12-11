using MinimalApi.Features.Examples.GetExampleChild;

namespace MinimalApi.Tests.Integration.Tests.Example
{
    [Collection("Endpoint")]
    public class FindExampleChildEndpointTests(RadEndpointFixture f)
    {
        [Fact]
        public async Task Given_ParentWithChildExists_ReturnsSuccess()
        {
            //Act
            var (h, r) = await f.Client.GetAsync<SearchExampleChildrenEndpoint, SearchExampleChildRequest, SearchExampleChildResponse>(new()
            {
                ParentId = 4,
                FirstName = "Luke",
                LastName = "Skywalker"
            });

            //Assert
            h.StatusCode.Should().Be(HttpStatusCode.OK);
            r.Should().BeOfType<SearchExampleChildResponse>();            
            r!.Data.Should().Contain(e => e.FirstName == "Luke" && e.LastName == "Skywalker");
            r.Message.Should().Be("Children found");
        }

        [Theory]
        [InlineData("", "", 4)]
        [InlineData("Luke", "Skywalker", 0)]
        [InlineData("Luke", "Skywalker", -1)]
        public async Task When_InvalidRequest_ReturnsProblem(string firstName, string lastName, int parentId)
        {
            //Act
            var (h, r) = await f.Client.GetAsync<SearchExampleChildrenEndpoint, SearchExampleChildRequest, ProblemDetails>(new()
            {
                ParentId = parentId,
                FirstName = firstName,
                LastName = lastName
            });
            //Assert
            h.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            r.Should().BeOfType<ProblemDetails>();
            r!.Should().NotBeNull();
        }

        [Fact]
        public async Task Given_ParentDoesntExists_ReturnsProblem()
        {
            //Act
            var (h, r) = await f.Client.GetAsync<SearchExampleChildrenEndpoint, SearchExampleChildRequest, ProblemDetails>(new()
            {
                ParentId = 1,
                FirstName = "Luke",
                LastName = "Skywalker"
            });
            //Assert
            h.StatusCode.Should().Be(HttpStatusCode.NotFound);
            r.Should().BeOfType<ProblemDetails>();
            r!.Should().NotBeNull();
        }
    }
}