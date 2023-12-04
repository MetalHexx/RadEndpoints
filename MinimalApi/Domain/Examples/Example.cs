namespace MinimalApi.Domain.Examples
{
    public record Example(string FirstName, string LastName, int Id = 0);

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
