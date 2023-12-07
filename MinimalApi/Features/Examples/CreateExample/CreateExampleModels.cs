using MinimalApi.Features.Examples._common;

namespace MinimalApi.Features.Examples.CreateExample
{
    public class CreateExampleRequest : ExampleDto { }

    public class CreateExampleValidator : AbstractValidator<CreateExampleRequest> 
    {
        public CreateExampleValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.Id).LessThanOrEqualTo(0);
            RuleFor(x => x).SetValidator(new ExampleValidator());
        }
    }
    public class CreateExampleResponse : EndpointResponse<ExampleDto> { }    
}
