using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.CreateExample
{
    public class CreateExampleMapper : IRadMapper<CreateExampleRequest, CreateExampleResponse, Example>
    {
        public CreateExampleResponse FromEntity(Example e) => new()
        {
            Data = new()
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName
            }
        };

        public Example ToEntity(CreateExampleRequest r) => new(r.FirstName, r.LastName);
    }
}