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
            RuleFor(e => e).NotNull();
            RuleFor(e => e.FirstName).NotEmpty();
            RuleFor(e => e.LastName).NotEmpty();
        }
    }
    public class CreateExampleResponse : RadResponse<ExampleDto> { }    
}
