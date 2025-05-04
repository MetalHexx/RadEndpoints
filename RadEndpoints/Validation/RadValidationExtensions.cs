using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace RadEndpoints.Validation
{
    public static class RadValidationExtensions
    {
        /// <summary>
        /// Adds a validation filter using FluentValidation for the specified request type.
        /// </summary>
        public static RouteHandlerBuilder WithRadValidation<TRequest>(this RouteHandlerBuilder builder) where TRequest : class
        {
            return builder.AddEndpointFilterFactory((context, next) =>
            {
                return async invocationContext =>
                {
                    var request = invocationContext.Arguments
                        .OfType<TRequest>()
                        .FirstOrDefault();

                    if (request is null)
                    {
                        return TypedResults.Problem(
                            title: "Request body cannot be null",
                            statusCode: StatusCodes.Status400BadRequest
                        );
                    }

                    var validator = invocationContext.HttpContext.RequestServices.GetService<IValidator<TRequest>>();

                    if (validator is null)
                    {
                        return TypedResults.Problem(
                            title: $"No validator registered for type {typeof(TRequest).Name}.",
                            statusCode: StatusCodes.Status500InternalServerError
                        );
                    }

                    var result = await validator.ValidateAsync(request);

                    if (!result.IsValid)
                    {
                        return TypedResults.ValidationProblem(
                            errors: result.ToDictionary(),
                            title: "Validation Error"
                        );
                    }

                    return await next(invocationContext);
                };
            });
        }
    }
}
