namespace RadEndpoints.Mediator
{
    public class RadMediatorRegistration
    {
        public Type EndpointType { get; set; } = default!;
        public Type RequestType { get; set; } = default!;
        public Type LoggerType { get; set; } = default!;
        public Type? MapperType { get; set; }
    }
}
