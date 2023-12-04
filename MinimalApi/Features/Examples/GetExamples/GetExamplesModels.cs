using MinimalApi.Features.Examples.Common.Models;
using MinimalApi.Http;

namespace MinimalApi.Features.Examples.GetExamples
{
    public class GetExamplesResponse : ApiResponse
    {
        public string Host { get; set; } = string.Empty;
        public IEnumerable<Example> Example { get; set; } = new List<Example>();
    }
}
