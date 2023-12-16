using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.DeleteExample
{
    public class DeleteExampleEndpoint(IExampleService s) : RadEndpoint<DeleteExampleRequest, DeleteExampleResponse>
    {
        public override void Configure()
        {
            Delete("/examples/{id}")
                .Produces<DeleteExampleResponse>(StatusCodes.Status200OK)
                .AddDocument(tag: Constants.ExamplesTag, desc: "Delete an example.");
        }

        public async override Task<IResult> Handle(DeleteExampleRequest r, CancellationToken ct)
        {
            await s.DeleteExample(r.Id);
            Response.Message = "Example deleted successfully";

            return Ok(Response);
        }
    }
}
