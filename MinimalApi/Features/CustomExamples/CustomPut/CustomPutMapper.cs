using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.CustomExamples.CustomPut
{
    public interface ICustomPutMapper : IMapper
    {
        CustomPutResponse FromEntity(Example entity);
        Example ToEntity(CustomPutRequest request, int id);
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

        public Example ToEntity(CustomPutRequest request, int id) => new(request.FirstName, request.LastName, id);
    }
}
