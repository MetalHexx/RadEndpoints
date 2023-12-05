using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.GetExample
{
    public class GetExampleEndpoint : Endpoint<GetExampleRequest, GetExampleResponse>
    {
        private readonly IExampleService _service;
        public GetExampleEndpoint(IExampleService exampleService) => _service = exampleService;
        public override void Configure()
        {
            Get("/examples/{id}")
                .Produces<GetExampleResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .AddSwagger(tag: Constants.ExamplesTag, desc: "Get an example by id");                
        }

        public async override Task<IResult> Handle(GetExampleRequest r, CancellationToken ct)
        {
            Response.Example = (await _service.GetExample(r.Id))!;
            Response.Message = "Example retrieved successfully";

            if(Response.Example is null)
            {
                return NotFound("Example not found");
            }
            return Ok(Response);
        }
    }
}
