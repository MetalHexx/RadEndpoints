namespace RadEndpoints.Mediator
{
    internal interface IRadMediatorRegistry
    {
        RadMediatorRegistration GetRegistration(Type endpointType);
        void RegisterEndpoints();
    }
}