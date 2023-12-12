namespace MinimalApi.Tests.Integration.Common
{
    public record RadTestResult<TResponse>(HttpResponseMessage Http, TResponse Content);
}