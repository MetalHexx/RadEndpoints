namespace RadEndpoints
{
    [Obsolete("Replace with a request wrapper in your api project.")]
    public abstract class RadRequest { }
    [Obsolete("Replace with a request wrapper in your api project.")]
    public abstract class RadRequest<T> : RadRequest
    {
        public abstract T Data { get; set; }
    }
}
