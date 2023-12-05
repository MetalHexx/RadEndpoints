using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.UpdateExample
{
    public class UpdateExampleRequest
    {
        public Example Example { get; set; } = null!;
    }

    public class UpdateExampleValidator : AbstractValidator<UpdateExampleRequest>
    {
        public UpdateExampleValidator()
        {
            RuleFor(x => x.Example).NotNull();
            RuleFor(x => x.Example.Id).GreaterThan(0);
            RuleFor(x => x.Example).SetValidator(new ExampleValidator());
        }
    }
    public class UpdateExampleResponse : EndpointResponse
    {
        public Example Example { get; set; } = null!;
    }
}
