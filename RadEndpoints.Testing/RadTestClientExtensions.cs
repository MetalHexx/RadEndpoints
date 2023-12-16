using System.Net.Http.Json;
using System.Text.Json;

namespace RadEndpoints.Testing
{
    public static class RadTestClientExtensions
    {
        public async static Task<RadTestResult<TResponse>> GetAsync<TEndpoint, TRequest, TResponse>(this HttpClient client, TRequest request)
            where TEndpoint : RadEndpoint
            where TRequest : RadRequest
        {
            var route = RadRouteExtensions.GetAndMapRoute<TEndpoint, TRequest>(request);
            var httpResponse = await client.GetAsync(route);
            client.Dispose();

            return new(httpResponse, await httpResponse.DeserializeJson<TResponse>());
        }

        public async static Task<RadTestResult<TResponse>> GetAsync<TEndpoint, TResponse>(this HttpClient client)
            where TEndpoint : RadEndpoint
        {
            var route = RadEndpoint.GetRoute<TEndpoint>();
            var httpResponse = await client.GetAsync(route);
            client.Dispose();

            return new(httpResponse, await httpResponse.DeserializeJson<TResponse>());
        }

        public async static Task<RadTestResult<TResponse>> DeleteAsync<TEndpoint, TRequest, TResponse>(this HttpClient client, TRequest request)
            where TEndpoint : RadEndpoint
            where TRequest : RadRequest
        {
            var route = RadRouteExtensions.GetAndMapRoute<TEndpoint, TRequest>(request);
            var httpResponse = await client.DeleteAsync(route);
            client.Dispose();

            return new(httpResponse, await httpResponse.DeserializeJson<TResponse>());
        }

        public async static Task<RadTestResult<TResponse>> PostAsync<TEndpoint, TRequest, TResponse>(this HttpClient client, TRequest request)
            where TEndpoint : RadEndpoint
            where TRequest : RadRequest
        {
            var route = RadRouteExtensions.GetAndMapRoute<TEndpoint, TRequest>(request);
            var httpResponse = await client.PostAsJsonAsync(route, request);
            client.Dispose();

            return new(httpResponse, await httpResponse.DeserializeJson<TResponse>());
        }

        public async static Task<RadTestResult<TResponse>> PutAsync<TRequest, TResponse>(this HttpClient client, string route, TRequest request)
            where TRequest : RadRequest
        {
            var httpResponse = await client.PutAsJsonAsync(route, request);
            client.Dispose();

            return new(httpResponse, await httpResponse.DeserializeJson<TResponse>());
        }

        public async static Task<RadTestResult<TResponse>> PutAsync<TEndpoint, TRequest, TResponse>(this HttpClient client, TRequest request)
            where TEndpoint : RadEndpoint
            where TRequest : RadRequest
        {            
            var httpRequest = RadRequestBuilder.BuildRequest<TEndpoint, TRequest>(client, request, HttpMethod.Put);
            var route = RadRouteExtensions.GetAndMapRoute<TEndpoint, TRequest>(request);
            httpRequest.RequestUri = client.BaseAddress!.Combine(route);

            var httpResponse = await client.SendAsync(httpRequest);
            client.Dispose();

            return new(httpResponse, await httpResponse.DeserializeJson<TResponse>());
        }

        private static async Task<TResponse> DeserializeJson<TResponse>(this HttpResponseMessage response)
        {
            try
            {
                return (await response!.Content!.ReadFromJsonAsync<TResponse>())!;
            }
            catch (JsonException ex)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                throw new RadTestException(stringResponse, response, ex);
            }
        }
    }
}