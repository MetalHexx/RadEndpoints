using MinimalApi.Domain.Examples;
using MinimalApi.Features.Examples._common;

namespace MinimalApi.Features.Examples.GetExample
{
    public class GetExampleEndpoint(IExampleService s) : RadEndpoint<GetExampleRequest, GetExampleResponse, GetExampleMapper>
    {
        public override void Configure()
        {
            Get("/examples/{id}")
                .Produces<GetExampleResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithDocument(tag: Constants.ExamplesTag, desc: "Get an example by id");
        }

        public async override Task Handle(GetExampleRequest r, CancellationToken ct)
        {
            // This condition is added only to demonstrate an issue. It is not meant to persists as a part of the
            // codebase and/or test suite.
            // ### START ###
            if (r.Id == 99)
            {
                // Set the response data, but do not use Send() method to send the response.
                // As per current implementation, RadEndpoint will send the response back.
                // But, the current implementation will not send this particular Response instance, rather
                // a different one - resulting in response that is not as expected.
                Response.Data = new ExampleDto
                {
                    FirstName = "Rando"
                };
                
                return;
            }
            // ### END ###
            
            var result = await s.GetExample(r.Id);

            result.Switch
            (
                example =>
                {
                    Response = Map.FromEntity(example);
                    Response.Message = "Example retrieved successfully";
                    Send();
                },
                notFound => SendProblem(notFound)
            );
        }
    }
}
