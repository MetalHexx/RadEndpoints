namespace RadEndpoints.Mediator
{
    public class RadMediatorRegistration
    {
        public Type EndpointType { get; set; } = null!;
        public Type EndpointKey { get; set; } = null!;
        public Type LoggerType { get; set; } = null!;
        public Type? MapperType { get; set; }
    }
}
