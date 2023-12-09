namespace MinimalApi.Tests.Integration.Common
{
    public class EndpointResponseSerializationException : Exception
    {
        public HttpResponseMessage HttpResponse { get; set; }
        public EndpointResponseSerializationException(string stringResponse, HttpResponseMessage httpResponse, Exception innerException) 
            : base($"\r\nProblem deserializing endpoint response.\r\n  Response Body: {stringResponse} \r\n", innerException) 
        {
            HttpResponse = httpResponse;
        }
    }
}
