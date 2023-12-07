using MinimalApi.Domain.Examples;
using MinimalApi.Features.Examples._common.Dtos;

namespace MinimalApi.Features.Examples.GetExamples
{
    public class GetExamplesResponse : EndpointResponse
    {
        public IEnumerable<ExampleDto> Examples { get; set; } = new List<ExampleDto>();
    }
}
