using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.GetExampleChild
{
    public class SearchChildExampleEndpoint(IExampleService s) : RadEndpoint<SearchChildExampleRequest, SearchChildExampleResponse, SearchChildExampleMapper>
    {
        public override void Configure()
        {
            Get("/examples/{parentId}/child")
                .Produces<SearchChildExampleResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithDocument(tag: Constants.ExamplesTag, desc: "Given a parent, find children by first or last name");
        }

        public override async Task<IResult> Handle(SearchChildExampleRequest r, CancellationToken ct)
        {
            var results = await s.SearchChildExample(r.ParentId, r.FirstName, r.LastName);

            return results.Match
            (
                children =>
                {
                    Response = Map.FromEntity(children);
                    Response.Message = "Children found";
                    return Send(Response);
                },
                notFound =>
                {
                    return SendNotFound(notFound.Message);
                }
            );
        }
    }
}
