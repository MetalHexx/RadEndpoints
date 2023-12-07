using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.GetExample
{
    public class GetExampleMapper : Mapper<GetExampleRequest, GetExampleResponse, Example>
    {
        public override GetExampleResponse FromEntity(Example entity) => new()
        {
            Data = new()
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName
            }
        };
        public override Example ToEntity(GetExampleRequest request) => throw new NotImplementedException();
    }
}