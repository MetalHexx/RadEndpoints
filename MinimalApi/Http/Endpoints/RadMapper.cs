namespace MinimalApi.Http.Endpoints
{
    public interface IRadMapper { };
    public abstract class RadMapper<TRequest, TResponse, TEntity>: IRadMapper where TResponse: RadResponse
    {
        public abstract TEntity ToEntity(TRequest r);
        public abstract TResponse FromEntity(TEntity e);
    }

    public abstract class RadMapper<TResponse, TEntity>: IRadMapper where TResponse: RadResponse
    {
        public abstract TResponse FromEntity(TEntity e);
    }
}
