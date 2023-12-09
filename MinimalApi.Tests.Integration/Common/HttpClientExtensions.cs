﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using System.Runtime.CompilerServices;
using System.Text.Json;
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

            return (httpResponse, await httpResponse.DeserializeJson<TResponse>());
        }

        public async static Task<(HttpResponseMessage HttpResponse, TResponse? EndpointResponse)> GetAsync<TEndpoint, TResponse>(this HttpClient client)
            where TEndpoint : Endpoint
        {
            var route = Endpoint.GetRoute<TEndpoint>();
            var httpResponse = await client.GetAsync(route);

            return (httpResponse, await httpResponse.DeserializeJson<TResponse>());
        }

        public async static Task<(HttpResponseMessage HttpResponse, TResponse? EndpointResponse)> DeleteAsync<TEndpoint, TRequest, TResponse>(this HttpClient client, TRequest request)
            where TEndpoint : Endpoint
        {
            var route = RouteExtensions.GetAndMapRoute<TEndpoint, TRequest>(request);
            var httpResponse = await client.DeleteAsync(route);

            return (httpResponse, await httpResponse.DeserializeJson<TResponse>());
        }

        public async static Task<(HttpResponseMessage HttpResponse, TResponse? EndpointResponse)> PostAsync<TEndpoint, TRequest, TResponse>(this HttpClient client, TRequest request)
            where TEndpoint : Endpoint
        {
            var route = Endpoint.GetRoute<TEndpoint>();
            var httpResponse = await client.PostAsJsonAsync(route, request);            
            return (httpResponse, await httpResponse.DeserializeJson<TResponse>());
        }

        public async static Task<(HttpResponseMessage HttpResponse, TResponse? EndpointResponse)> PutAsync<TEndpoint, TRequest, TResponse>(this HttpClient client, TRequest request)
            where TEndpoint : Endpoint
        {
            var route = Endpoint.GetRoute<TEndpoint>();
            var httpResponse = await client.PutAsJsonAsync(route, request);

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
                throw new EndpointResponseSerializationException(stringResponse, response, ex);
            }
        } 
    }
}