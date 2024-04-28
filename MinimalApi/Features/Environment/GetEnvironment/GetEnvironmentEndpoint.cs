namespace MinimalApi.Features.Environment.GetEnvironment
{
    public class GetEnvironmentEndpoint : RadEndpointWithoutRequest<GetEnvironmentResponse, GetEnvironmentMapper>
    {
        public override void Configure()
        {
            Get("/environment")
                .Produces<GetEnvironmentResponse>(StatusCodes.Status200OK)
                .WithDocument(tag: "Environment", desc: "Get information about the application environment.  Example of an endpoint with a mapper but no request");
        }

        public async override Task Handle(CancellationToken ct)
        {
            Response = Map.FromEntity(Env);
            Response.Message = "Environment information retrieved successfully";
            Send();
        }
    }
}
