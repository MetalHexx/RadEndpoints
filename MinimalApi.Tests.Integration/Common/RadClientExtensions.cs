using MinimalApi.Http.Endpoints;
using System.Text.Json;

namespace MinimalApi.Tests.Integration.Common
{
    public static class RadClientExtensions
    {
        public async static Task<(HttpResponseMessage HttpResponse, TResponse? EndpointResponse)> GetAsync<TEndpoint, TRequest, TResponse>(this HttpClient client, TRequest request)
            where TEndpoint : RadEndpoint
            where TRequest : RadRequest
        {
            var route = RadRouteExtensions.GetAndMapRoute<TEndpoint, TRequest>(request);
            using var httpResponse = await client.GetAsync(route);
            client.Dispose();

            return (httpResponse, await httpResponse.DeserializeJson<TResponse>());
        }

        public async static Task<(HttpResponseMessage HttpResponse, TResponse? EndpointResponse)> GetAsync<TEndpoint, TResponse>(this HttpClient client)
            where TEndpoint : RadEndpoint
        {
            var route = RadEndpoint.GetRoute<TEndpoint>();
            using var httpResponse = await client.GetAsync(route);
            client.Dispose();

            return (httpResponse, await httpResponse.DeserializeJson<TResponse>());
        }

        public async static Task<(HttpResponseMessage HttpResponse, TResponse? EndpointResponse)> DeleteAsync<TEndpoint, TRequest, TResponse>(this HttpClient client, TRequest request)
            where TEndpoint : RadEndpoint
            where TRequest : RadRequest
        {
            var route = RadRouteExtensions.GetAndMapRoute<TEndpoint, TRequest>(request);
            using var httpResponse = await client.DeleteAsync(route);
            client.Dispose();

            return (httpResponse, await httpResponse.DeserializeJson<TResponse>());
        }

        public async static Task<(HttpResponseMessage HttpResponse, TResponse? EndpointResponse)> PostAsync<TEndpoint, TRequest, TResponse>(this HttpClient client, TRequest request)
            where TEndpoint : RadEndpoint
            where TRequest : RadRequest
        {
            var route = RadEndpoint.GetRoute<TEndpoint>();
            using var httpResponse = await client.PostAsJsonAsync(route, request);
            client.Dispose();

            return (httpResponse, await httpResponse.DeserializeJson<TResponse>());
        }

        public async static Task<(HttpResponseMessage HttpResponse, TResponse? EndpointResponse)> PutAsync<TRequest, TResponse>(this HttpClient client, string route, TRequest request)
            where TRequest : RadRequest
        {            
            using var httpResponse = await client.PutAsJsonAsync(route, request);
            client.Dispose();

            return (httpResponse, await httpResponse.DeserializeJson<TResponse>());
        }

        public async static Task<(HttpResponseMessage HttpResponse, TResponse? EndpointResponse)> PutAsync<TEndpoint, TRequest, TResponse>(this HttpClient client, TRequest request)
            where TEndpoint : RadEndpoint
            where TRequest : RadRequest
        {   
            var httpRequest = RadRequestBuilder.BuildRequest<TEndpoint, TRequest>(client, request, HttpMethod.Put);
            using var httpResponse = await client.SendAsync(httpRequest);
            client.Dispose();

            return (httpResponse, await httpResponse.DeserializeJson<TResponse>());
        }

        private static async Task<TResponse?> DeserializeJson<TResponse>(this HttpResponseMessage response)
        {
            try
            {
                return await response.Content.ReadFromJsonAsync<TResponse>();                
            }
            catch (JsonException ex)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                throw new RadTestException(stringResponse, response, ex);
            }
        }
    }
}