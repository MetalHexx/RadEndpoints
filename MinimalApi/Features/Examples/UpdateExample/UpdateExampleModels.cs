using MinimalApi.Features.Examples._common;

namespace MinimalApi.Features.Examples.UpdateExample
{
    public class UpdateExampleRequest : ExampleDto { }

    public class UpdateExampleValidator : AbstractValidator<UpdateExampleRequest>
    {
        public UpdateExampleValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x).SetValidator(new ExampleValidator());
        }
    }
    public class UpdateExampleResponse : EndpointResponse<ExampleDto> { }
}
