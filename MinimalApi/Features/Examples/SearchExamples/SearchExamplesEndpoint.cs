using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.SearchExamples
{
    public class SearchExamplesEndpoint() : RadEndpoint<SearchExamplesRequest, SearchExamplesResponse, SearchExamplesMapper>
    {
        public override void Configure()
        {
            Get("/examples/search")
                .Produces<SearchExamplesResponse>(StatusCodes.Status200OK)
                .WithDocument(tag: Constants.ExamplesTag, desc: "Search for examples");
        }

        public async override Task<IResult> Handle(SearchExamplesRequest r, CancellationToken ct)
        {
            var examples = await Service<IExampleService>().FindExamples(r.FirstName, r.LastName);
            
            Response = Map.FromEntity(examples);
            Response.Message = examples.Any()
                ? "Examples found successfully"
                : "No examples found";
            
            return Ok(Response);
        }
    }
}
