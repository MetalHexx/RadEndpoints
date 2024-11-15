namespace MinimalApi.Features.Redirect.WithPayload.RedirectWithPayloadEndpoint
{
    public class RedirectOrPayloadEndpoint : RadEndpoint<RedirectOrPayloadRequest, RedirectOrPayloadResponse>
    {
        public override void Configure()
        {
            Post("/redirect/payload")
                .Produces<RedirectOrPayloadResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status302Found)
                .WithDocument(tag: "Redirect", desc: "A redirect payload that may return a redirect or a response model.");
        }

        public override async Task Handle(RedirectOrPayloadRequest r, CancellationToken ct)
        {
            await Task.CompletedTask;

            if (r.ShouldRedirect) 
            {
                SendRedirect("http://fake.url/", permanent: false, preserveMethod: false);
                return;
            }
            Response = new();
            Send();
        }
    }
}