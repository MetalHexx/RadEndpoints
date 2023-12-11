namespace MinimalApi.Http.Endpoints
{
    public abstract class RadRequest<T>
    {
        public abstract T Data { get; set; }
    }
}
