using Endpoint = MinimalApi.Http.Endpoints.Endpoint;

namespace MinimalApi.Tests.Integration.Common
{
    public static class HttpClientExtensions
    {
        public async static Task<(HttpResponseMessage HttpResponse, TResponse? EndpointResponse)> GetAsync<TEndpoint, TRequest, TResponse>(this HttpClient client, TRequest request)
            where TEndpoint : Endpoint
        {
            var route = RouteExtensions.GetAndMapRoute<TEndpoint, TRequest>(request);
            var httpResponse = await client.GetAsync(route);
            var endpointResponse = await httpResponse.Content.ReadFromJsonAsync<TResponse>();

            return (httpResponse, endpointResponse);
        }
    }
}
