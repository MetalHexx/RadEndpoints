using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.DeleteExample
{
    public class DeleteExampleEndpoint : Endpoint<DeleteExampleRequest, DeleteExampleResponse>
    {
        private readonly IExampleService _service;

        public DeleteExampleEndpoint(IExampleService service) => _service = service;
        public override void Configure()
        {
            Delete("/examples/{id}")
                .Produces<DeleteExampleResponse>(StatusCodes.Status200OK)
                .AddSwagger(tag: Constants.ExamplesTag, desc: "Delete an example.");
        }

        public async override Task<IResult> Handle(DeleteExampleRequest r, CancellationToken ct)
        {
            await _service.DeleteExample(r.Id);
            Response.Message = "Example deleted successfully";

            return Ok(Response);
        }
    }
}
