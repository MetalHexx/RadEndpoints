namespace RadEndpoints.Mediator
{
    public interface IRadMediator
    {
        Task CallHandlerAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
            where TRequest : class
            where TResponse : new();
    }
}