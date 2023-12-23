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
            var results = await s.GetExamples();

            return results.Match
            (
                examples =>
                {
                    Response = Map.FromEntity(examples);
                    Response.Message = "Examples retrieved successfully";
                    return Send(Response);
                },
                notFound =>
                {
                    Logger.Log(LogLevel.Warning, "Examples not found");
                    return SendNotFound(notFound.Message);
                }
            );
        }
    }
}
