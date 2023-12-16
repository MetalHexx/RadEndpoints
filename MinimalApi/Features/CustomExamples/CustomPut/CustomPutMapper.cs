using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.CustomExamples.CustomPut
{
    public interface ICustomPutMapper : IRadMapper
    {
        CustomPutResponse FromEntity(Example entity);
        Example ToEntity(CustomPutRequest request);
    }
    public class CustomPutMapper: ICustomPutMapper
    {
        public CustomPutResponse FromEntity(Example entity) => new()
        {
            Data = new()
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName
            }
        };

        public Example ToEntity(CustomPutRequest request) => new(request.Data.FirstName, request.Data.LastName, request.Id);
    }
}
