using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.SearchExamples
{
    public class SearchExamplesEndpoint(IExampleService s) : RadEndpoint<SearchExamplesRequest, SearchExamplesResponse, SearchExamplesMapper>
    {
        public override void Configure()
        {
            Get("/examples/search")
                .Produces<SearchExamplesResponse>(StatusCodes.Status200OK)
                .WithDocument(tag: Constants.ExamplesTag, desc: "Search for examples");
        }

        public async override Task<IResult> Handle(SearchExamplesRequest r, CancellationToken ct)
        {
            var results = await s.FindExamples(r.FirstName, r.LastName);

            return results.Match
            (
                examples =>
                {
                    Response = Map.FromEntity(examples);
                    Response.Message = "Examples found successfully";
                    return Ok(Response);
                },
                notFound => NotFound(notFound.Message)
            );
        }
    }
}
