﻿using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.DeleteExample
{
    public class DeleteExampleEndpoint(IExampleService s) : RadEndpoint<DeleteExampleRequest, DeleteExampleResponse>
    {
        public override void Configure()
        {
            Delete("/examples/{id}")
                .Produces<DeleteExampleResponse>(StatusCodes.Status200OK)
                .WithDocument(tag: Constants.ExamplesTag, desc: "Delete an example.");
        }

        public async override Task<IResult> Handle(DeleteExampleRequest r, CancellationToken ct)
        {
            var result = await s.DeleteExample(r.Id);

            return result.Match
            (
                none => 
                {
                    Response.Message = "Example deleted successfully";
                    return Send(Response);
                },
                notFound => SendNotFound(notFound.Message)
            );
        }
    }
}
