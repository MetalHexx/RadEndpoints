using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.UpdateExample
{
    public class UpdateExampleMapper : Mapper<UpdateExampleRequest, UpdateExampleResponse, Example>
    {
        public override UpdateExampleResponse FromEntity(Example entity) => new()
        {
            Data = new()
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName
            }
        };

        public override Example ToEntity(UpdateExampleRequest request) => new(request.FirstName, request.LastName);
    }
}