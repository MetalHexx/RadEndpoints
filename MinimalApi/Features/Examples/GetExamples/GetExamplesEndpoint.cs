using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.GetExamples
{
    public class GetExamplesEndpoint : EndpointWithoutRequest<GetExamplesResponse>
    {
        private readonly IExampleService _service;
        public GetExamplesEndpoint(IExampleService service) => _service = service;

        public override void Configure()
        {
            Get("/examples")
                .Produces<GetExamplesResponse>(StatusCodes.Status200OK)
                .AddSwagger(tag: Constants.ExamplesTag, desc: "Create a new example.");
        }

        public override async Task<IResult> Handle(CancellationToken c)
        {
            Logger.Log(LogLevel.Information, "This is a test log message.");

            Response.Host = HttpContext?.Request.Host.ToString() ?? "";
            Response.Example = await _service.GetExamples();
            Response.Message = "Successfully retrieved examples.";

            return Ok(Response);
        }
    }
}
