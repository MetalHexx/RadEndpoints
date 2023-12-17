using MinimalApi.Features.Examples._common;

namespace MinimalApi.Features.CustomExamples.CustomPut
{
    public class CustomPutRequest : RadRequest<CustomPutDto>
    {
        [FromRoute]
        public int Id { get; set; }
        [FromBody]
        public override CustomPutDto Data { get; set; } = null!;
    }
    public class CustomPutDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class CustomPutRequestValidator : AbstractValidator<CustomPutRequest>
    {
        public CustomPutRequestValidator()
        {
            RuleFor(x => x.Data.FirstName).NotEmpty();
            RuleFor(x => x.Data.LastName).NotEmpty();
        }
    }
    public class CustomPutResponse : RadResponse<ExampleDto> { }
}
