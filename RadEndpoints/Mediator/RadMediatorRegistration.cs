namespace RadEndpoints.Mediator
{
    public class RadMediatorRegistration
    {
        public Type EndpointType { get; set; } = null!;
        public Type RequestType { get; set; } = null!;
        public Type LoggerType { get; set; } = null!;
        public Type? MapperType { get; set; }
    }
}
