using MinimalApi.Features.Examples._common;

namespace MinimalApi.Features.Examples.SearchExamples
{
    public class SearchExamplesRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }

    public class SearchExamplesRequestValidator: AbstractValidator<SearchExamplesRequest>
    {
        public SearchExamplesRequestValidator()
        {
            RuleFor(e => e.FirstName)
                .NotEmpty()
                .When(r => string.IsNullOrEmpty(r.LastName))
                .WithMessage("At least one of the properties (FirstName or LastName) is required.");

            RuleFor(e => e.LastName)
                .NotEmpty()
                .When(r => string.IsNullOrEmpty(r.FirstName))
                .WithMessage("At least one of the properties (FirstName or LastName) is required.");
        }
    }

    public class SearchExamplesResponse : RadResponse<IEnumerable<ExampleDto>> { }
}