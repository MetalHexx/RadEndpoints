using Microsoft.AspNetCore.Mvc;
using MinimalApi.Features.Examples._common;

namespace MinimalApi.Features.Examples.GetExampleChild
{
    public class SearchExampleChildRequest : RadRequest
    {
        [FromRoute]
        public int ParentId { get; set; }
        [FromQuery]
        public string? FirstName { get; set; }
        [FromQuery]
        public string? LastName { get; set; }
    }

    public class SearchExampleChildrenValidator : AbstractValidator<SearchExampleChildRequest>
    {
        public SearchExampleChildrenValidator()
        {
            RuleFor(x => x.ParentId).GreaterThan(0);
            RuleFor(x => x.FirstName).NotEmpty().When(x => string.IsNullOrEmpty(x.LastName));
            RuleFor(x => x.LastName).NotEmpty().When(x => string.IsNullOrEmpty(x.FirstName));
        }
    }

    public class SearchExampleChildResponse : RadResponse<IEnumerable<ExampleDto>> { }
}
