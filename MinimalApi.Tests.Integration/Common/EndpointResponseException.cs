namespace MinimalApi.Tests.Integration.Common
{
    public class EndpointResponseException : Exception
    {
        public HttpResponseMessage HttpResponse { get; set; }
        public EndpointResponseException(string stringResponse, HttpResponseMessage httpResponse, Exception innerException) 
            : base($"\r\nProblem deserializing endpoint response.\r\nResponse Body: {stringResponse}\r\nStatus Code: {httpResponse.StatusCode}\r\nReason Phrase: {httpResponse.ReasonPhrase}", innerException) 
        {
            HttpResponse = httpResponse;
        }
    }
}
