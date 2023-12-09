using MinimalApi.Features.Examples._common;

namespace MinimalApi.Features.Examples.UpdateExample
{
    public class UpdateExampleRequest : ExampleDto { }

    public class UpdateExampleValidator : AbstractValidator<UpdateExampleRequest>
    {
        public UpdateExampleValidator()
        {
            RuleFor(e => e).NotNull();
            RuleFor(e => e.Id).GreaterThan(0);
            RuleFor(e => e.FirstName).NotEmpty();
            RuleFor(e => e.LastName).NotEmpty();
        }
    }
    public class UpdateExampleResponse : RadResponse<ExampleDto> { }
}
