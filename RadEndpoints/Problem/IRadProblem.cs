namespace RadEndpoints
{
    public interface IRadProblem;
    public sealed record ForbiddenError(string Message) : IRadProblem;
    public sealed record ConflictError(string Message) : IRadProblem;
    public sealed record NotFoundError(string Message) : IRadProblem;
    public sealed record ValidationError(string Name, string Message) : IRadProblem;
    public sealed record InternalError(string Message, Exception? Exception) : IRadProblem;
    public sealed record ExternalError(string Message, Exception? Exception) : IRadProblem;
}