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
}
