using MinimalApi.Domain.Examples;
using MinimalApi.Features.Examples._common;

namespace MinimalApi.Features.Examples.GetExampleChild
{
    public class SearchExampleChildMapper : RadMapper<SearchExampleChildResponse, IEnumerable<Example>>
    {
        public override SearchExampleChildResponse FromEntity(IEnumerable<Example> e) => new()
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
