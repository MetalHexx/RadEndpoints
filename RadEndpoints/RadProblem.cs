namespace RadEndpoints
{
    public static class Problem
    {
        public static ForbiddenError Auth(string message) => new(message);
        public static ConflictError Conflict(string message) => new(message);
        public static NotFoundError NotFound(string message) => new(message);
        public static ValidationError Validation(string key, string value) => new(key, value);
        public static InternalError Internal(string message, Exception? exception = null) => new(message, exception);
        public static ExternalError External(string message, Exception? exception = null) => new(message, exception);
    }
    public abstract record RadProblem(string Message);
    public sealed record ForbiddenError(string Message) : RadProblem(Message);
    public sealed record ConflictError(string Message) : RadProblem(Message);
    public sealed record NotFoundError(string Message) : RadProblem(Message);
    public sealed record ValidationError(string Key, string Message) : RadProblem(Message);
    public sealed record InternalError(string Message, Exception? Exception) : RadProblem(Message);
    public sealed record ExternalError(string Message, Exception? Exception) : RadProblem(Message);
}
