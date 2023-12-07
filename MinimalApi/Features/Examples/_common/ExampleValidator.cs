namespace MinimalApi.Features.Examples._common
{
    internal class ExampleValidator : AbstractValidator<ExampleDto>
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
