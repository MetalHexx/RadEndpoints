namespace RadEndpoints
{
    /// <summary>
    /// Marker interface for RadEndpoints mappers.
    /// </summary>
    public interface IRadMapper { };

    /// <summary>
    /// Mapper interface for converting domain entities to response DTOs.
    /// Use this interface when you only need one-way mapping from entities to responses (e.g., GET endpoints).
    /// </summary>
    /// <typeparam name="TResponse">The response model type returned by the endpoint</typeparam>
    /// <typeparam name="TEntity">The domain entity type</typeparam>
    public interface IRadMapper<TResponse, TEntity> : IRadMapper
        where TResponse : new()
    {
        /// <summary>
        /// Converts a domain entity to a response DTO.
        /// </summary>
        /// <param name="e">The domain entity to convert</param>
        /// <returns>The response DTO</returns>
        public abstract TResponse FromEntity(TEntity e);
    }

    /// <summary>
    /// Mapper interface for bi-directional conversion between request DTOs, response DTOs, and domain entities.
    /// Use this interface when you need two-way mapping (e.g., POST, PUT, PATCH endpoints).
    /// </summary>
    /// <typeparam name="TRequest">The request model type received by the endpoint</typeparam>
    /// <typeparam name="TResponse">The response model type returned by the endpoint</typeparam>
    /// <typeparam name="TEntity">The domain entity type</typeparam>
    public interface IRadMapper<TRequest, TResponse, TEntity> : IRadMapper
        where TRequest : class
        where TResponse : new()
    {
        /// <summary>
        /// Converts a request DTO to a domain entity.
        /// Use this method to transform incoming request data into domain objects for business logic operations.
        /// </summary>
        /// <param name="r">The request DTO to convert</param>
        /// <returns>The domain entity</returns>
        public abstract TEntity ToEntity(TRequest r);

        /// <summary>
        /// Converts a domain entity to a response DTO.
        /// Use this method to transform domain objects into API response data.
        /// </summary>
        /// <param name="e">The domain entity to convert</param>
        /// <returns>The response DTO</returns>
        public abstract TResponse FromEntity(TEntity e);
    }
}