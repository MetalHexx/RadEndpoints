namespace MinimalApi.Http.Endpoints
{
    public class RadResponse
    {
        public string Message { get; set; } = string.Empty;
    }

    public class RadResponse<T>: RadResponse
    {
        public T? Data { get; set; } = default!;
    }
}
