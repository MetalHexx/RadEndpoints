using MinimalApi.Features.Examples.SearchExamples;

namespace MinimalApi.Tests.Integration.Tests.Example
{
    [Collection("Endpoint")]
    public class SearchExampleEndpointTests(RadEndpointFixture f)
    {

        [Theory]
        [InlineData("Luke", "Skywalker")]
        [InlineData("", "Skywalker")]
        [InlineData("Luke", "")]
        public async Task Given_ExampleExists_ReturnsSuccess(string firstName, string lastName) 
        {
            //Arrange
            var request = new SearchExamplesRequest
            {
                FirstName = firstName,
                LastName = lastName
            };

            //Act
            var (h, r) = await f.Client.GetAsync<SearchExamplesEndpoint, SearchExamplesRequest, SearchExamplesResponse>(request); 

            //Assert
            h.StatusCode.Should().Be(HttpStatusCode.OK);
            r.Should().BeOfType<SearchExamplesResponse>();
            r!.Data.Should().Contain(e => e.FirstName == "Luke" && e.LastName == "Skywalker");
            r.Data!.First().LastName.Should().Be("Skywalker");
        }

        [Fact]
        public async Task When_SearchEmpty_ReturnsProblem()
        {
            //Arrange
            var request = new SearchExamplesRequest
            {
                FirstName = string.Empty,
                LastName = string.Empty
            };

            //Act
            var (h, r) = await f.Client.GetAsync<SearchExamplesEndpoint, SearchExamplesRequest, ProblemDetails>(request); 

            //Assert
            h.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            r.Should().BeOfType<ProblemDetails>();
            r!.Extensions.Should().ContainKey("FirstName");
            r.Extensions.Should().ContainKey("LastName");
        }
    }
}