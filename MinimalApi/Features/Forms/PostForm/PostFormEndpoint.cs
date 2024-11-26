using RadEndpoints;

namespace MinimalApi.Features.Forms.PostForm
{
    public class PostFormEndpoint : RadEndpoint<PostFormRequest, PostFormResponse>
    {
        public override void Configure()
        {
            Post("/forms")
                .Accepts<PostFormRequest>("multipart/form-data")
                .Produces<PostFormResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .DisableAntiforgery()
                .WithTags("test forms")
                .WithDescription("test forms");
        }

        public override async Task Handle(PostFormRequest r, CancellationToken ct)
        {
            Response = new()
            {
                HeaderValue = HttpContext.Request.Headers.Referer
            };
            Send();
        }
    }
}