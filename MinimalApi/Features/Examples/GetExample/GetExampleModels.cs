using MinimalApi.Features.Examples._common;

namespace MinimalApi.Features.Examples.GetExample
{
    public class GetExampleRequest : RadRequest
    {
        public int Id { get; set; }
    }

    public class GetExampleRequestValidator : AbstractValidator<GetExampleRequest>
    {
        public GetExampleRequestValidator()
        {
            RuleFor(e => e.Id).GreaterThan(0);
        }
    }

    public class GetExampleResponse : RadResponse<ExampleDto> { }
}
