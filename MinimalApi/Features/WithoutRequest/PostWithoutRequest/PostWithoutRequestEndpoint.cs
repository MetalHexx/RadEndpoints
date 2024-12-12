namespace MinimalApi.Features.WithoutRequest.PostWithoutRequest
{
    public class PostWithoutRequestEndpoint : RadEndpointWithoutRequest<PostWithoutRequestResponse>
    {
        public override void Configure()
        {
            Post("/without-request")
                .Produces<PostWithoutRequestResponse>(StatusCodes.Status200OK)
                .WithDocument(tag: "WithoutRequest", desc: "Example of an endpoint with no request and no mapper");
        }
        public async override Task Handle(CancellationToken ct)
        {
            await Task.CompletedTask;
            Response.Message = "Success!";
            Send();
        }
    }
}
