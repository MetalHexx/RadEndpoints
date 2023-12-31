﻿using MinimalApi.Domain.Examples;

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

        public override async Task Handle(GetExamplesRequest r, CancellationToken c)
        {   
            var results = await s.GetExamples();

            results.Switch
            (
                examples =>
                {
                    Response = Map.FromEntity(examples);
                    Response.Message = "Examples retrieved successfully";
                    Send();
                },
                notFound =>
                {
                    Logger.Log(LogLevel.Warning, "Examples not found");
                    SendProblem(notFound);
                }
            );
        }
    }
}
