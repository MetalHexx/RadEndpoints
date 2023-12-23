using MinimalApi.Domain.Examples;

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
