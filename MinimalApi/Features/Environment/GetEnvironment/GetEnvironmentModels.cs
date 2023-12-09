namespace MinimalApi.Features.Environment.GetEnvironment
{
    public class GetEnvironmentResponse : RadResponse
    {
        public string EnvironmentName { get; set; } = string.Empty;
        public string ApplicationName { get; set; } = string.Empty;
    }
}