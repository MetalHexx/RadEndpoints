using MinimalApi.Features.Examples.Common.Services;
using MinimalApi.Http;

namespace MinimalApi.Features.Examples.GetExamples
{
    public class GetExamplesEndpoint : EndpointWithoutRequest<GetExamplesResponse>
    {
        private readonly IExampleService _service;
        public GetExamplesEndpoint(IExampleService service) => _service = service;

        public override void Configure()
        {
            RouteBuilder
                .MapGet("/examples", async (CancellationToken ct) => await Handle(ct))
                .Produces<GetExamplesResponse>(StatusCodes.Status200OK);
        }

        public override async Task<IResult> Handle(CancellationToken c)
        {
            Logger.Log(LogLevel.Information, "This is a test log message.");

            Response.Host = HttpContext?.Request.Host.ToString() ?? "";
            Response.Example = await _service.GetExamples();
            Response.Message = "Successfully retrieved examples.";

            return TypedResults.Ok(Response);
        }
    }
}
