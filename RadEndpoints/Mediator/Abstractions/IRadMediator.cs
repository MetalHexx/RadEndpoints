namespace RadEndpoints.Mediator
{
    public interface IRadMediator
    {
        Task CallHandler<TRequest, TResponse>(Type endpointType, TRequest request, CancellationToken cancellationToken)
            where TRequest : class
            where TResponse : new();

        Task CallHandler<TResponse>(Type endpointType, CancellationToken cancellationToken)
            where TResponse : new();
    }
}