namespace MinimalApi.Features.Redirect.Temp
{
    public class TempRedirectEndpoint : RadEndpoint
    {
        public override void Configure()
        {
            RouteBuilder
                .MapGet(SetRoute("/redirect/temp"), () => Handle())
                .Produces(StatusCodes.Status307TemporaryRedirect)
                .WithDocument(tag: "Redirect", desc: "Example of a redirect endpoint.");
        }

        public IResult Handle()
        {
            return TypedResults.Redirect("http://fake.url/", permanent: false, preserveMethod: true);
        }
    }
}