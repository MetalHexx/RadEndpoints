using System.Net;
using System.Text.Json;

namespace MinimalApi.Features.CustomBase._common
{
    public class CustomBaseResponse : IResult
    {
        public string Message { get; init; } = string.Empty;
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public async Task ExecuteAsync(HttpContext httpContext)
        {
            var response = httpContext.Response;
            response.StatusCode = (int)StatusCode;
            response.ContentType = "application/json";

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var result = JsonSerializer.Serialize(this, GetType(), options);

            await response.WriteAsync(result);
        }
    }
}
