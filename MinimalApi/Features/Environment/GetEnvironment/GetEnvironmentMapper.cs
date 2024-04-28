namespace MinimalApi.Features.Environment.GetEnvironment
{
    public class GetEnvironmentMapper : IRadMapper<GetEnvironmentResponse, IWebHostEnvironment>
    {
        public GetEnvironmentResponse FromEntity(IWebHostEnvironment e) => new()
        {
            EnvironmentName = e.EnvironmentName,
            ApplicationName = e.ApplicationName
        };
    }
}
