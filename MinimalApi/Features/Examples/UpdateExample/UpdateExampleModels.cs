using MinimalApi.Features.Examples._common;

namespace MinimalApi.Features.Examples.UpdateExample
{
    public class ExampleUpdateDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
    public class UpdateExampleRequest
    {
        [FromRoute]
        public int Id { get; set; }
        [FromBody]
        public ExampleUpdateDto Example { get; set; } = null!;
    }

    public class UpdateExampleValidator : AbstractValidator<UpdateExampleRequest>
    {
        public UpdateExampleValidator()
        {
            RuleFor(e => e).NotNull();
            RuleFor(e => e.Id).GreaterThan(0);
            RuleFor(e => e.Example.FirstName).NotEmpty();
            RuleFor(e => e.Example.LastName).NotEmpty();
        }
    }
    public class UpdateExampleResponse : RadResponse<ExampleDto> { }
}
