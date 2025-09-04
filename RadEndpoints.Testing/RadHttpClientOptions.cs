using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace RadEndpoints.Testing
{
    /// <summary>
    /// Configuration options for RadEndpoints testing HTTP client operations
    /// </summary>
    public class RadHttpClientOptions 
    {
        /// <summary>
        /// Additional headers to include in the HTTP request
        /// </summary>
        public HeaderDictionary Headers { get; set; } = [];

        /// <summary>
        /// Custom JSON serializer options for request/response serialization.
        /// If not provided, default JsonSerializerOptions will be used.
        /// </summary>
        public JsonSerializerOptions? JsonSerializerOptions { get; set; }
    }
}