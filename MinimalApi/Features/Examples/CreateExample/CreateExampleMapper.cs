using MinimalApi.Domain.Examples;
using static CreateExampleValidator;

namespace MinimalApi.Features.Examples.CreateExample
{
    public class CreateExampleMapper : Mapper<CreateExampleRequest, CreateExampleResponse, Example>
    {
        public override CreateExampleResponse FromEntity(Example entity) => new()
        {
            Data = new()
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName
            }
        };

        public override Example ToEntity(CreateExampleRequest request) => new(request.FirstName, request.LastName);
    }
}