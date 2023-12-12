using System.Reflection;
using System.Text;
using System.Web;
using System.Text.Json;
using System.Net.Mime;
using MinimalApi.Http.Endpoints;

namespace MinimalApi.Tests.Integration.Common
{
    public static class RadRequestBuilder
    {
        public static HttpRequestMessage BuildRequest<TEndpoint, TRequest>(HttpClient client, TRequest requestModel, HttpMethod method)
            where TEndpoint : RadEndpoint
        {
            if(HasRequestModelAttributes<TRequest>())
            {
                return BuildRequestFromAttributes<TEndpoint, TRequest>(client, requestModel, method);
            }
            return new HttpRequestMessage();
        }

        private static HttpRequestMessage BuildRequestFromAttributes<TEndpoint, TRequest>(HttpClient client, TRequest requestModel, HttpMethod method) where TEndpoint : RadEndpoint
        {
            var routeTemplate = RadEndpoint.GetRoute<TEndpoint>();
            var queryValues = HttpUtility.ParseQueryString(string.Empty);
            var headers = new HeaderDictionary();
            var formContent = new MultipartFormDataContent();
            StringContent body = null!;

            foreach (var property in typeof(TRequest).GetProperties())
            {
                var propertyValue = property.GetValue(requestModel)?.ToString();

                if (string.IsNullOrEmpty(propertyValue)) continue;

                var attribute = property.GetCustomAttributes().FirstOrDefault();

                if (attribute is null) throw new RadTestException("Make sure you add binding attributes to every property on your request model.  Possible attributes you can use include: [FromRoute] [FromQuery] [FromBody] [FromForm] [FromHeader]");

                switch (attribute)
                {
                    case FromRouteAttribute:
                        routeTemplate = routeTemplate.MapRouteParam(property.Name, propertyValue);
                        break;
                    case FromQueryAttribute:
                        queryValues[property.Name] = propertyValue;
                        break;
                    case FromHeaderAttribute:
                        headers.Add(property.Name, propertyValue);
                        break;
                    case FromFormAttribute:
                        formContent.Add(new StringContent(propertyValue), property.Name);
                        break;
                    case FromBodyAttribute:
                        body = SerializeContent(property.GetValue(requestModel)!);
                        break;
                }
            }
            var httpRequest = new HttpRequestMessage
            {
                Method = method,
                RequestUri = client.BaseAddress!.AppendUri(routeTemplate, queryValues),
                Content = body ?? formContent as HttpContent
            };
            httpRequest.AddHeaders(headers);
            httpRequest.AddContent(body, formContent);
            return httpRequest;
        }

        private static StringContent SerializeContent(object value, JsonSerializerOptions? options = null)
        {
            var json = JsonSerializer.Serialize(value, options ?? new JsonSerializerOptions());
            return new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
        }

        private static void AddHeaders(this HttpRequestMessage requestMessage, HeaderDictionary headers)
        {
            foreach (var header in headers)
            {
                requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToString());
            }
        }

        private static void AddContent(this HttpRequestMessage httpRequest, StringContent? body, MultipartFormDataContent formContent)
        {
            if (body is not null)
            {
                httpRequest.Content = body;
                return;
            }
            if (formContent.Any())
            {
                httpRequest.Content = formContent;
            }
        }

        private static string MapRouteParam(this string url, string name, string value) =>
            url.Replace($"{{{name}}}", HttpUtility.UrlEncode(value), StringComparison.OrdinalIgnoreCase);

        public static bool HasRequestModelAttributes<TRequest>()
        {
            return typeof(TRequest).GetProperties()
                .SelectMany(property => property.GetCustomAttributes())
                .Any(attribute => attribute is FromRouteAttribute ||
                                  attribute is FromQueryAttribute ||
                                  attribute is FromHeaderAttribute ||
                                  attribute is FromFormAttribute ||
                                  attribute is FromBodyAttribute);
        }
    }
}