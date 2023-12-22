using FluentValidation.Results;

namespace RadEndpoints.Validation
{
    public static class ValidationResultExtensions
    {
        public static IDictionary<string, string[]> ToDictionary(this ValidationResult validationResult)
        {
            return validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    failureGroup => failureGroup.Key,
                    failureGroup => failureGroup.Select(failure => failure.ErrorMessage).ToArray()
                );
        }
    }
}
