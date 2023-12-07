using MinimalApi.Features.Examples._common;

namespace MinimalApi.Features.Examples.CreateExample
{
    public class CreateExampleRequest : ExampleDto { }

    public class CreateExampleValidator : AbstractValidator<CreateExampleRequest> 
    {
        public CreateExampleValidator()
        {
            RuleFor(e => e).NotNull();
            RuleFor(e => e.Id).LessThanOrEqualTo(0);
            RuleFor(e => e).SetValidator(new ExampleValidator());
        }
    }
    public class CreateExampleResponse : EndpointResponse<ExampleDto> { }    
}
