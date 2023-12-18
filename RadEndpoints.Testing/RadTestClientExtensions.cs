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
            return await client.SendAsync<TEndpoint, TRequest, TResponse>(request, HttpMethod.Get);
        }

        public async static Task<RadTestResult<TResponse>> DeleteAsync<TEndpoint, TRequest, TResponse>(this HttpClient client, TRequest request)
            where TEndpoint : RadEndpoint
            where TRequest : RadRequest
        {
            return await client.SendAsync<TEndpoint, TRequest, TResponse>(request, HttpMethod.Delete);
        }

        public async static Task<RadTestResult<TResponse>> PostAsync<TEndpoint, TRequest, TResponse>(this HttpClient client, TRequest request)
            where TEndpoint : RadEndpoint
            where TRequest : RadRequest
        {
            return await client.SendAsync<TEndpoint, TRequest, TResponse>(request, HttpMethod.Post);
        }

        public async static Task<RadTestResult<TResponse>> PutAsync<TEndpoint, TRequest, TResponse>(this HttpClient client, TRequest request)
            where TEndpoint : RadEndpoint
            where TRequest : RadRequest
        {
            return await client.SendAsync<TEndpoint, TRequest, TResponse>(request, HttpMethod.Put);
        }

        public async static Task<RadTestResult<TResponse>> PatchAsync<TEndpoint, TRequest, TResponse>(this HttpClient client, TRequest request)
            where TEndpoint : RadEndpoint
            where TRequest : RadRequest
        {
            return await client.SendAsync<TEndpoint, TRequest, TResponse>(request, HttpMethod.Patch);
        }

        public async static Task<RadTestResult<TResponse>> SendAsync<TEndpoint, TRequest, TResponse>(this HttpClient client, TRequest request, HttpMethod method)
            where TEndpoint : RadEndpoint
            where TRequest : RadRequest
        {
            var httpRequest = RadRequestBuilder.BuildRequest<TEndpoint, TRequest>(client, request, method);

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