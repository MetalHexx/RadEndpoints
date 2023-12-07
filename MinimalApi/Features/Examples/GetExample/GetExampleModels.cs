using MinimalApi.Features.Examples._common;

namespace MinimalApi.Features.Examples.GetExample
{
    public class GetExampleRequest
    {
        public int Id { get; set; }
    }

    public class GetExampleRequestValidator : AbstractValidator<GetExampleRequest>
    {
        public GetExampleRequestValidator()
        {
            RuleFor(r => r.Id).GreaterThan(0);
        }
    }

    public class GetExampleResponse : EndpointResponse<ExampleDto> { }
}
