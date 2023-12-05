using MinimalApi.Domain.Examples;

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
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }

    public class GetExampleResponse : EndpointResponse
    {
        public Example Example { get; set; } = null!;
    }
}
