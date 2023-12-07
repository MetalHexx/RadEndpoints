using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.GetExample
{
    public class GetExampleMapper : Mapper<GetExampleRequest, GetExampleResponse, Example>
    {
        public override GetExampleResponse FromEntity(Example e) => new()
        {
            Data = new()
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName
            }
        };
        public override Example ToEntity(GetExampleRequest r) => throw new NotImplementedException();
    }
}