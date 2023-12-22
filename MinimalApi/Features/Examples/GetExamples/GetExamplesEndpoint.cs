using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.GetExamples
{
    public class GetExamplesEndpoint(IExampleService s) : RadEndpoint<GetExamplesRequest, GetExamplesResponse, GetExamplesMapper>
    {
        public override void Configure()
        {
            Get("/examples")
                .Produces<GetExamplesResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithDocument(tag: Constants.ExamplesTag, desc: "Create a new example.");
        }

        public override async Task<IResult> Handle(GetExamplesRequest r, CancellationToken c)
        {
            Logger.Log(LogLevel.Information, "This is an example log message.");
            var results = await s.GetExamples();

            return results.Match
            (
                examples =>
                {
                    Response = Map.FromEntity(examples);
                    Response.Message = "Examples retrieved successfully";
                    return Ok(Response);
                },
                notFound =>
                {
                    return NotFound(notFound.Message);
                }
            );
        }
    }
}
