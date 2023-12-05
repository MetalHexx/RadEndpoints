using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.SearchExamples
{
    public class SearchExamplesEndpoint : EndpointWithQuery<SearchExamplesResponse>
    {
        private readonly IExampleService _service;
        public SearchExamplesEndpoint(IExampleService service) => _service = service;
        public override void Configure()
        {
            RouteBuilder.MapGet("/examples/search", async (string? firstName, string? lastName, CancellationToken ct) => await Handle(firstName, lastName, ct))
                .Produces<SearchExamplesResponse>(StatusCodes.Status200OK)
                .AddSwagger(tag: Constants.ExamplesTag, desc: "Search for examples");
        }

        public async Task<IResult> Handle(string? firstName, string? lastName, CancellationToken ct)
        {
            Response.Examples = await _service.FindExamples(firstName, lastName);
            Response.Message = "Examples found successfully";

            if(!Response.Examples.Any())
            {
                return NotFound("No examples found");
            }
            return Ok(Response);
        }
    }
}
