using FluentValidation;
using MinimalApi.Features.Examples.Common.Models;

namespace MinimalApi.Features.Examples.Common
{
    internal class ExampleValidator : AbstractValidator<Example>
    {
        public ExampleValidator()
        {
            RuleFor(x => x).NotNull()
                .WithMessage("Example cannot be null");

            RuleFor(x => x!.FirstName).NotEmpty()
                .WithMessage("First name cannot be empty");

            RuleFor(x => x!.LastName).NotEmpty()
                .WithMessage("Last name cannot be empty");
        }
    }
}
