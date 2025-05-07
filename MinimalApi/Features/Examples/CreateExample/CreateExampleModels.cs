using MinimalApi.Features.Examples._common;

namespace MinimalApi.Features.Examples.CreateExample
{
    public class CreateExampleRequest: RadRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class CreateExampleValidator : AbstractValidator<CreateExampleRequest> 
    {
        public CreateExampleValidator()
        {
            RuleFor(e => e).NotNull().WithMessage("Request cannot be null");
            RuleFor(e => e.FirstName).NotEmpty().WithMessage("First name cannot be empty");
            RuleFor(e => e.LastName).NotEmpty().WithMessage("Last name cannot be empty");
        }
    }
    public class CreateExampleResponse : RadResponse<ExampleDto> { }    
}
