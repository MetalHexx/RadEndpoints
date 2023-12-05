using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.SearchExamples
{
    public class SearchExamplesEndpoint : Endpoint<SearchExamplesRequest, SearchExamplesResponse>
    {
        private readonly IExampleService _service;
        public SearchExamplesEndpoint(IExampleService service) => _service = service;

        public override void Configure()
        {
            Get("/examples/search")
                .Produces<SearchExamplesResponse>(StatusCodes.Status200OK)
                .AddSwagger(tag: Constants.ExamplesTag, desc: "Search for examples");
        }

        public async override Task<IResult> Handle(SearchExamplesRequest r, CancellationToken ct)
        {
            Response.Examples = await _service.FindExamples(r.FirstName, r.LastName);
            Response.Message = "Examples found successfully";

            if (!Response.Examples.Any())
            {
                return NotFound("No examples found");
            }
            return Ok(Response);
        }
    }
}
