
using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.ProblemExample
{
    public class ProblemExampleEndpoint : RadEndpoint<ProblemExampleRequest, ProblemExampleResponse>
    {
        public override void Configure()
        {
            Get("/examples/problem")
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithDocument(tag: Constants.ExamplesTag, desc: "Example of how to return a problem details model using the SendProblem helper");
        }

        public override Task Handle(ProblemExampleRequest r, CancellationToken ct)
        {
            SendProblem(TypedResults.Problem(
                title: "Problem Example",
                statusCode: StatusCodes.Status400BadRequest,
                type: "https://example.com/problem",
                detail: "This is an example of a problem details response.",
                instance: "/examples/problem",
                extensions: new Dictionary<string, object?>
               {
                   { "example", "This is an example of a problem details response." },
                   { "example2", "This is an example of a problem details response." },
               }
            ));
            return Task.CompletedTask;
        }
    }

    public class ProblemExampleWithoutRequestEndpoint : RadEndpointWithoutRequest<ProblemExampleResponse>
    {
        public override void Configure()
        {
            Get("/examples/problem/Without/Request")
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithDocument(tag: Constants.ExamplesTag, desc: "Example of how to return a problem details model using the SendProblem helper");
        }

        public override Task Handle(CancellationToken ct)
        {
            SendProblem(TypedResults.Problem(
                title: "Problem Example",
                statusCode: StatusCodes.Status400BadRequest,
                type: "https://example.com/problem",
                detail: "This is an example of a problem details response.",
                instance: "/examples/problem",
                extensions: new Dictionary<string, object?>
               {
                   { "example", "This is an example of a problem details response." },
                   { "example2", "This is an example of a problem details response." },
               }
            ));
            return Task.CompletedTask;
        }
    }
}
