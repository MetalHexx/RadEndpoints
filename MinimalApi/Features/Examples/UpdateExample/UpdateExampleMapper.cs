using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.UpdateExample
{
    public class UpdateExampleMapper : IRadMapper<UpdateExampleRequest, UpdateExampleResponse, Example>
    {
        public UpdateExampleResponse FromEntity(Example entity) => new()
        {
            Data = new()
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName
            }
        };

        public Example ToEntity(UpdateExampleRequest request) => 
            new(request.Data.FirstName, request.Data.LastName, request.Id);
    }
}