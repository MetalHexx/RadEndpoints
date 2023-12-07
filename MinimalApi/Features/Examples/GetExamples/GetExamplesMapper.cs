using MinimalApi.Domain.Examples;
using MinimalApi.Features.Examples._common.Dtos;

namespace MinimalApi.Features.Examples.GetExamples
{
    public class GetExamplesMapper : Mapper<GetExamplesResponse, IEnumerable<Example>>
    {
        public override GetExamplesResponse FromEntity(IEnumerable<Example> entity) => new()
        {
            Data = entity.Select(e => new ExampleDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName
            })
        };
    }
}