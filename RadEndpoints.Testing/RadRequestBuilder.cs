using System.Reflection;
using System.Text;
using System.Web;
using System.Text.Json;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace RadEndpoints.Testing
{
    public static class RadRequestBuilder
    {
        public static HttpRequestMessage BuildRequest<TEndpoint>(this HttpClient client, HttpMethod method, RadHttpClientOptions? options = null)
            where TEndpoint : RadEndpoint
        {
            var routeTemplate = RadEndpoint.GetRoute<TEndpoint>();
            if (routeTemplate.HasParameterPlaceholders())
            {
                throw new RadTestException($"\r\nProblem executing: ({method}) {routeTemplate} \r\nThe route has parameter placeholders but there is no request model for this endpoint");
            }
            HttpRequestMessage httpRequestMessage = new()
            {
                Method = method,
                RequestUri = client.BaseAddress!.Combine(routeTemplate)
            };
            
            if(options?.Headers is not null)
            {
                httpRequestMessage.AddHeaders(options.Headers);
            }
            return httpRequestMessage;
        }

        public static HttpRequestMessage BuildRequest<TEndpoint, TRequest>(this HttpClient client, TRequest requestModel, HttpMethod method, RadHttpClientOptions? options = null)
            where TEndpoint : RadEndpoint
        {
            return HasRequestModelAttributes<TRequest>()
                ? client.BuildRequestFromAttributes<TEndpoint, TRequest>(requestModel, method, options)
                : client.BuildRequestWithoutAttributes<TEndpoint, TRequest>(requestModel, method, options);
        }

        private static HttpRequestMessage BuildRequestWithoutAttributes<TEndpoint, TRequest>(this HttpClient client, TRequest requestModel, HttpMethod method, RadHttpClientOptions? options = null)
        {
            var routeTemplate = RadEndpoint.GetRoute<TEndpoint>();
            if(routeTemplate.HasParameterPlaceholders())
            {
                throw new RadTestException($"\r\nProblem executing {requestModel?.GetType().Name}: ({method}) {routeTemplate} \r\nThe route has parameter placeholders but the {requestModel?.GetType().Name} is missing attributes.  \r\nEnsure you have attributes if you have route or query params.  \r\nPossible attributes: [FromRoute] [FromQuery] [FromBody] [FromForm] [FromHeader]");
            }
            HttpRequestMessage httpRequestMessage = new()
            {
                Method = method,
                RequestUri = client.BaseAddress!.Combine(routeTemplate),
                Content = requestModel!.ToStringContent(options?.JsonSerializerOptions)
            };
            if (options?.Headers is not null)
            {
                httpRequestMessage.AddHeaders(options.Headers);
            }
            return httpRequestMessage;
        }

        private static HttpRequestMessage BuildRequestFromAttributes<TEndpoint, TRequest>(
            this HttpClient client,
            TRequest requestModel,
            HttpMethod method,
            RadHttpClientOptions? options = null
        ) where TEndpoint : RadEndpoint
        {
            var routeTemplate = RadEndpoint.GetRoute<TEndpoint>();
            var queryFromAttribs = HttpUtility.ParseQueryString(string.Empty);
            var headersFromAttribs = new HeaderDictionary();
            MultipartFormDataContent? formContent = null;
            StringContent? body = null;

            foreach (var property in typeof(TRequest).GetProperties())
            {
                var attribute = property
                    .GetCustomAttributes(inherit: true)
                    .FirstOrDefault(a =>
                        a is FromRouteAttribute
                        || a is FromQueryAttribute
                        || a is FromHeaderAttribute
                        || a is FromFormAttribute
                        || a is FromBodyAttribute
                    ) ?? throw new RadTestException(
                        $"Missing binding attribute on property '{property.Name}'. Expected one of: [FromRoute], [FromQuery], [FromBody], [FromForm], [FromHeader]."
                    );

                var rawValue = property.GetValue(requestModel);

                if (rawValue is null &&
                    attribute is not FromBodyAttribute
                    && attribute is not FromQueryAttribute
                    && attribute is not FromHeaderAttribute
                    && attribute is not FromFormAttribute)
                {
                    continue;
                }

                var propertyValue = rawValue?.ToString();

                if (attribute is FromRouteAttribute)
                {
                    if (string.IsNullOrEmpty(propertyValue))
                    {
                        throw new RadTestException($"Route parameter '{property.Name}' must not be null or empty.");
                    }

                    routeTemplate = routeTemplate.MapRouteParam(property.Name, propertyValue);
                }
                else if (attribute is FromQueryAttribute)
                {
                    if (rawValue is not null)
                    {
                        queryFromAttribs[property.Name] = propertyValue ?? string.Empty;
                    }
                }
                else if (attribute is FromHeaderAttribute)
                {
                    if (rawValue is not null)
                    {
                        headersFromAttribs.Add(property.Name, propertyValue ?? string.Empty);
                    }
                }
                else if (attribute is FromFormAttribute)
                {
                    if (rawValue is not null)
                    {
                        formContent ??= new MultipartFormDataContent();
                        var stringContent = new StringContent(propertyValue ?? string.Empty);
                        stringContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = $"\"{property.Name}\""
                        };
                        formContent.Add(stringContent);
                    }
                }
                else if (attribute is FromBodyAttribute)
                {
                    body = rawValue?.ToStringContent(options?.JsonSerializerOptions) ?? string.Empty.ToStringContent(options?.JsonSerializerOptions);
                }
            }

            var httpRequest = new HttpRequestMessage
            {
                Method = method,
                RequestUri = client.BaseAddress!.Combine(routeTemplate, queryFromAttribs)
            };

            if (options?.Headers is not null)
            {
                httpRequest.AddHeaders(options.Headers);
            }

            if (headersFromAttribs.Count != 0)
            {
                httpRequest.AddHeaders(headersFromAttribs);
            }

            if (body is not null && formContent is not null)
            {
                throw new RadTestException("Cannot have both [FromBody] and [FromForm] in the same request model.");
            }

            if (body is not null)
            {
                httpRequest.Content = body;
            }

            if (formContent is not null)
            {
                httpRequest.Content = formContent;
            }

            return httpRequest;
        }

        private static StringContent ToStringContent(this object value, JsonSerializerOptions? options = null)
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

        private static bool HasParameterPlaceholders(this string routeTemplate) => routeTemplate.Contains('{', StringComparison.OrdinalIgnoreCase);

        private static string MapRouteParam(this string url, string name, string value) =>
            url.Replace($"{{{name}}}", HttpUtility.UrlEncode(value), StringComparison.OrdinalIgnoreCase);

        public static bool HasRequestModelAttributes<TRequest>()
        {
            return typeof(TRequest)
                .GetProperties()
                .SelectMany(property => property.GetCustomAttributes())
                .Any(attribute => attribute is FromRouteAttribute ||
                                  attribute is FromQueryAttribute ||
                                  attribute is FromHeaderAttribute ||
                                  attribute is FromFormAttribute ||
                                  attribute is FromBodyAttribute);
        }
    }
}