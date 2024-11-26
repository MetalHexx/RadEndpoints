using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.PatchExample
{
    public class PatchExampleMapper : IRadMapper<PatchExampleRequest, PatchExampleResponse, Example>
    {
        public PatchExampleResponse FromEntity(Example entity) => new()
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName
        };

        public Example ToEntity(PatchExampleRequest r) => new(r.Example.FirstName, r.Example.LastName, r.Id);
    }
}