namespace MinimalApi.Http.Filters
{
    internal class ValidationFilter<TRequest> : IEndpointFilter where TRequest : class
    {
        private readonly IValidator<TRequest> _validator;

        public ValidationFilter(IValidator<TRequest> validator)
        {
            _validator = validator;
        }

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

            var validationResult = await _validator.ValidateAsync(validatable);

            if (!validationResult.IsValid)
            {
                var problem = TypedResults.Problem
                (
                    title: "There were errors in the request body",
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