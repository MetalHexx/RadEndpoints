using MinimalApi.Features.Examples._common;

namespace MinimalApi.Features.Examples.UpdateExample
{
    public class UpdateExampleRequest: RadRequest<ExampleUpdateDto>
    {
        [FromRoute]
        public int Id { get; set; }
        [FromBody]
        public override ExampleUpdateDto Data { get; set; } = null!;
    }
    public class ExampleUpdateDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class UpdateExampleValidator : AbstractValidator<UpdateExampleRequest>
    {
        public UpdateExampleValidator()
        {
            RuleFor(e => e).NotNull();
            RuleFor(e => e.Id).GreaterThan(0);
            RuleFor(e => e.Data.FirstName).NotEmpty();
            RuleFor(e => e.Data.LastName).NotEmpty();
        }
    }
    public class UpdateExampleResponse : RadResponse<ExampleDto> { }
}
