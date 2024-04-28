namespace RadEndpoints
{
    [Obsolete("Replace with a response wrapper in your api project.")]
    public class RadResponse
    {
        public string Message { get; set; } = string.Empty;
    }

    [Obsolete("Replace with a response wrapper in your api project.")]
    public class RadResponse<T> : RadResponse
    {
        public T? Data { get; set; } = default!;
    }
}