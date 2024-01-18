using MinimalApi.Features.CustomBase._common;

namespace MinimalApi.Features.CustomBase.CustomGet
{
    public class CustomGetRequest : CustomBaseRequest
    {
        public int Id { get; set; }
    }
    public class CustomGetResponse : CustomBaseResponse
    {
        public string Data { get; set; } = string.Empty;
    }
}
