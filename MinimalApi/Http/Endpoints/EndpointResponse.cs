namespace MinimalApi.Http.Endpoints
{
    public class EndpointResponse
    {
        public string Message { get; set; } = string.Empty;
    }

    public class EndpointResponse<T>: EndpointResponse
    {
        public T? Data { get; set; } = default!;
    }
}
