namespace MinimalApi.Features.Environment.GetEnvironment
{
    public class GetEnvironmentEndpoint : RadEndpoint<GetEnvironmentRequest, GetEnvironmentResponse>
    {
        public override void Configure()
        {
            Get("/environment")
                .Produces<GetEnvironmentResponse>(StatusCodes.Status200OK)
                .WithDocument(tag: "Environment", desc: "Get information about the application environment.");
        }

        public async override Task<IResult> Handle(GetEnvironmentRequest r, CancellationToken ct)
        {
            Response.EnvironmentName = Env.EnvironmentName;
            Response.ApplicationName = Env.ApplicationName;
            Response.Message = "Environment information retrieved successfully";

            return Send(Response);
        }
    }
}
