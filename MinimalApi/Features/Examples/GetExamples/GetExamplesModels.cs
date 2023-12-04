using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.GetExamples
{
    public class GetExamplesResponse : EndpointResponse
    {
        public string Host { get; set; } = string.Empty;
        public IEnumerable<Example> Example { get; set; } = new List<Example>();
    }
}
