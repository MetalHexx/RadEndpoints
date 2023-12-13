namespace RadEndpoints.Testing
{
    public record RadTestResult<TResponse>(HttpResponseMessage Http, TResponse Content);
}