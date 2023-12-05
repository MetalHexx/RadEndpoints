using MinimalApi.Domain.Examples;

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
            RuleFor(request => request.FirstName)
                .NotEmpty()
                .When(request => string.IsNullOrEmpty(request.LastName))
                .WithMessage("At least one of the properties (FirstName or LastName) is required.");

            RuleFor(request => request.LastName)
                .NotEmpty()
                .When(request => string.IsNullOrEmpty(request.FirstName))
                .WithMessage("At least one of the properties (FirstName or LastName) is required.");
        }
    }

    public class SearchExamplesResponse : EndpointResponse
    {
        public IEnumerable<Example> Examples { get; set; } = null!;
    }
}