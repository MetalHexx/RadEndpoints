using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.CreateExample
{
    public class CreateExampleMapper : Mapper<CreateExampleRequest, CreateExampleResponse, Example>
    {
        public override CreateExampleResponse FromEntity(Example e) => new()
        {
            Data = new()
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName
            }
        };

        public override Example ToEntity(CreateExampleRequest r) => new(r.FirstName, r.LastName);
    }
}