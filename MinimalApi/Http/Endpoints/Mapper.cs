namespace MinimalApi.Http.Endpoints
{
    public interface IMapper { };
    public abstract class Mapper<TRequest, TResponse, TEntity>: IMapper where TResponse: RadResponse
    {
        public abstract TEntity ToEntity(TRequest r);
        public abstract TResponse FromEntity(TEntity e);
    }

    public abstract class Mapper<TResponse, TEntity>: IMapper where TResponse: RadResponse
    {
        public abstract TResponse FromEntity(TEntity e);
    }
}
