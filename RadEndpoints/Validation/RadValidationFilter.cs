using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace RadEndpoints
{
    public class RadValidationFilter<TRequest>(IValidator<TRequest> v) : IEndpointFilter where TRequest : class
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validatable = context.Arguments.SingleOrDefault(x => x?.GetType() == typeof(TRequest)) as TRequest;

            if (validatable is null)
            {
                return TypedResults.Problem
                (
                    title: "Request body cannot be null",
                    statusCode: StatusCodes.Status400BadRequest
                );
            }

            var validationResult = await v.ValidateAsync(validatable);

            if (!validationResult.IsValid)
            {
                var problem = TypedResults.Problem
                (
                    title: "Validation Error",
                    statusCode: StatusCodes.Status400BadRequest
                );
                validationResult.Errors.ForEach(error =>
                    problem.ProblemDetails.Extensions.Add(error.PropertyName, error.ErrorMessage));

                return problem;
            }

            return await next(context);
        }
    }
}