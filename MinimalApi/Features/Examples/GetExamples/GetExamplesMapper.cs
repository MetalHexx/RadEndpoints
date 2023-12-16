using MinimalApi.Domain.Examples;
using MinimalApi.Features.Examples._common;

namespace MinimalApi.Features.Examples.GetExamples
{
    public class GetExamplesMapper : IRadMapper<GetExamplesResponse, IEnumerable<Example>>
    {
        public GetExamplesResponse FromEntity(IEnumerable<Example> e) => new()
        {
            Data = e.Select(e => new ExampleDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName
            })
        };
    }
}