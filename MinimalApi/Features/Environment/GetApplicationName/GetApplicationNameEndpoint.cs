namespace MinimalApi.Features.Environment.GetApplicationName
{
    public class GetApplicationNameEndpoint : RadEndpointWithoutRequest<GetApplicationNameResponse>
    {
        public override void Configure()
        {
            Get("/environment/application")
                .Produces<GetApplicationNameResponse>(StatusCodes.Status200OK)
                .WithDocument(tag: "Environment", desc: "Get the application name.  Example of an endpoint with no request and no mapper");
        }

        public async override Task Handle(CancellationToken ct)
        {
            Response.ApplicationName = Env.ApplicationName;
            Response.Message = "Application name retrieved successfully";
            Send();
        }
    }
}
