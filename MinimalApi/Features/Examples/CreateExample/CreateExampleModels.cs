using MinimalApi.Features.Examples.Common.Models;
using MinimalApi.Features.Examples.Common;

namespace MinimalApi.Features.Examples.CreateExample
{
    public class CreateExampleRequest
    {
        public Example Example { get; set; } = null!;
    }

    public class CreateExampleValidator : AbstractValidator<CreateExampleRequest>
    {
        public CreateExampleValidator()
        {
            RuleFor(x => x.Example).SetValidator(new ExampleValidator());

            RuleFor(x => x.Example).NotNull()
                .WithMessage("Example cannot be null");

            RuleFor(x => x.Example!.Id).LessThanOrEqualTo(0)
                .WithMessage("New examples cannot have an id")
                .When(x => x.Example is not null);
        }
    }

    public class CreateExampleResponse : ApiResponse
    {
        public Example? Example { get; set; } = null!;
    }
}
