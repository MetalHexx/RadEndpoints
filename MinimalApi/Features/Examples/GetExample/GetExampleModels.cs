using MinimalApi.Features.Examples._common;

namespace MinimalApi.Features.Examples.GetExample
{
    public class GetExampleRequest
    {
        [FromRoute]
        public int Id { get; set; }
    }

    public class GetExampleRequestValidator : AbstractValidator<GetExampleRequest>
    {
        public GetExampleRequestValidator()
        {
            RuleFor(e => e.Id).GreaterThan(0);
        }
    }

    public class GetExampleResponse
    {
        public ExampleDto Data { get; set; } = null!;
        public string Message { get; set; } = "Example retrieved successfully";
    }
}
