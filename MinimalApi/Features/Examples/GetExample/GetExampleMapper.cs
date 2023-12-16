using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.GetExample
{
    public class GetExampleMapper : IRadMapper<GetExampleRequest, GetExampleResponse, Example>
    {
        public GetExampleResponse FromEntity(Example e) => new()
        {
            Data = new()
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName
            }
        };
        public Example ToEntity(GetExampleRequest r) => throw new NotImplementedException();
    }
}