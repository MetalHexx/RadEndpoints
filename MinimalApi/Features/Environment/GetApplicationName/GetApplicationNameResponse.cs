namespace MinimalApi.Features.Environment.GetApplicationName
{
    public class GetApplicationNameResponse : RadResponse
    {
        public string ApplicationName { get; set; } = string.Empty;
    }
}