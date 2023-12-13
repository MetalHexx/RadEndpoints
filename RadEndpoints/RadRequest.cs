namespace RadEndpoints
{
    public abstract class RadRequest { }
    public abstract class RadRequest<T> : RadRequest
    {
        public abstract T Data { get; set; }
    }
}
