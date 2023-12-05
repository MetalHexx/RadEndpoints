using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.SearchExamples
{
    public class SearchExamplesResponse : EndpointResponse
    {
        public IEnumerable<Example> Examples { get; set; } = null!;
    }
}
