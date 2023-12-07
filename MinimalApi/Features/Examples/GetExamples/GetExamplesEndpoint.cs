using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.GetExamples
{
    public class GetExamplesEndpoint : EndpointWithoutRequest<GetExamplesResponse, GetExamplesMapper>
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
            Logger.Log(LogLevel.Information, "This is an example log message.");
            var examples = await _service.GetExamples();
            Response = Map.FromEntity(examples);

            return Ok(Response);
        }
    }
}
