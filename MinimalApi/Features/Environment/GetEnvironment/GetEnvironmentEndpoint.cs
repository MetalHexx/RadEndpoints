namespace MinimalApi.Features.Environment.GetEnvironment
{
    public class GetEnvironmentEndpoint : RadEndpointWithoutRequest<GetEnvironmentResponse>
    {
        public override void Configure()
        {
            Get("/environment")
                .Produces<GetEnvironmentResponse>(StatusCodes.Status200OK)
                .AddDocument(tag: "Environment", desc: "Get information about the application environment.");
        }

        public async override Task<IResult> Handle(CancellationToken ct)
        {
            Response.EnvironmentName = Env.EnvironmentName;
            Response.ApplicationName = Env.ApplicationName;
            Response.Message = "Environment information retrieved successfully";

            return Ok(Response);
        }
    }
}
