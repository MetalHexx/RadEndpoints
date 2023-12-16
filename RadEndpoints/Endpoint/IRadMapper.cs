namespace RadEndpoints
{
    public interface IRadMapper { };

    public interface IRadMapper<TResponse, TEntity> : IRadMapper where TResponse : RadResponse
    {
        public abstract TResponse FromEntity(TEntity e);
    }

    public interface IRadMapper<TRequest, TResponse, TEntity> : IRadMapper where TResponse : RadResponse
    {
        public abstract TEntity ToEntity(TRequest r);
        public abstract TResponse FromEntity(TEntity e);
    }
}