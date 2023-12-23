using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Lite.LightPut
{
    public interface ICustomPutMapper : IRadMapper<CustomPutRequest, CustomPutResponse, Example> { }
    public class LitePutMapper: ICustomPutMapper
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
