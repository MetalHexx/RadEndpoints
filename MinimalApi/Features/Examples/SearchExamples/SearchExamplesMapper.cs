using MinimalApi.Domain.Examples;
using MinimalApi.Features.Examples._common;

namespace MinimalApi.Features.Examples.SearchExamples
{
    public class SearchExamplesMapper : IRadMapper<SearchExamplesResponse, IEnumerable<Example>>
    {
        public SearchExamplesResponse FromEntity(IEnumerable<Example> e) => new()
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