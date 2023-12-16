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
                .AddDocument(tag: Constants.ExamplesTag, desc: "Given a parent, find children by first or last name");
        }

        public override async Task<IResult> Handle(SearchChildExampleRequest r, CancellationToken ct)
        {
            var children = await s.SearchChildExample(r.ParentId, r.FirstName, r.LastName);

            if(children.Any() == false)
            {
                return NotFound("No children found");
            }  

            Response = Map.FromEntity(children);
            Response.Message = "Children found";

            return Ok(Response);
        }
    }
}
