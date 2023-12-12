using MinimalApi.Features.Examples._common;

namespace MinimalApi.Features.Examples.GetExampleChild
{
    public class SearchChildExampleRequest : RadRequest
    {
        [FromRoute]
        public int ParentId { get; set; }
        [FromQuery]
        public string? FirstName { get; set; }
        [FromQuery]
        public string? LastName { get; set; }
    }

    public class SearchChildExampleValidator : AbstractValidator<SearchChildExampleRequest>
    {
        public SearchChildExampleValidator()
        {
            RuleFor(x => x.ParentId).GreaterThan(0);
            RuleFor(x => x.FirstName).NotEmpty().When(x => string.IsNullOrEmpty(x.LastName));
            RuleFor(x => x.LastName).NotEmpty().When(x => string.IsNullOrEmpty(x.FirstName));
        }
    }

    public class SearchChildExampleResponse : RadResponse<IEnumerable<ExampleDto>> { }
}
