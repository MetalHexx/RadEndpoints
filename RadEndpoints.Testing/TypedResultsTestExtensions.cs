using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;

namespace RadEndpoints.Testing
{
    /// <summary>
    /// Extension methods for RadEndpoint instances to facilitate unit testing
    /// by providing easy access to HttpContext.Items results using the standard TypedResults pattern.
    /// </summary>
    public static class TypedResultsTestExtensions
    {
        /// <summary>
        /// Gets the result of the specified type from the HttpContext.Items after endpoint execution.
        /// This method extracts results that were set by Send(), SendCreatedAt(), or error methods.
        /// Throws a RadTestException if the result of the given type is not found.
        /// </summary>
        /// <typeparam name="T">The expected result type (e.g., Ok&lt;ResponseType&gt;, Created&lt;ResponseType&gt;, ProblemHttpResult)</typeparam>
        /// <param name="endpoint">The RadEndpoint instance</param>
        /// <returns>The result of type T</returns>
        /// <exception cref="RadTestException">Thrown when the result of the given type is not found.</exception>
        public static T GetResult<T>(this RadEndpoint endpoint) where T : class
        {
            if (endpoint.HttpContext.Items.TryGetValue(RadConstants.Context_Key_Result, out var result) && result is T typedResult)
            {
                return typedResult;
            }

            throw new RadTestException($"TypedResult of type {typeof(T).Name} was not returned by the endpoint.");
        }

        /// <summary>
        /// Gets a problem result from the HttpContext.Items after endpoint execution.
        /// This method extracts problems that were set by SendProblem() methods.
        /// Throws a RadTestException if the problem of the given type is not found.
        /// </summary>
        /// <typeparam name="T">The expected problem type (e.g., ProblemHttpResult, ValidationProblem, or IRadProblem implementations)</typeparam>
        /// <param name="endpoint">The RadEndpoint instance</param>
        /// <returns>The problem of type T</returns>
        /// <exception cref="RadTestException">Thrown when the problem of the given type is not found.</exception>
        public static T GetProblem<T>(this RadEndpoint endpoint) where T : class
        {
            if (endpoint.HttpContext.Items.TryGetValue(RadConstants.Context_Key_RadProblem, out var problem) && problem is T typedProblem)
            {
                return typedProblem;
            }

            throw new RadTestException($"TypedProblem of type {typeof(T).Name} was not returned by the endpoint.");
        }

        /// <summary>
        /// Checks if the endpoint has any result set in HttpContext.Items.
        /// </summary>
        /// <param name="endpoint">The RadEndpoint instance</param>
        /// <returns>True if a result is set, false otherwise</returns>
        public static bool HasResult(this RadEndpoint endpoint)
        {
            return endpoint.HttpContext.Items.ContainsKey(RadConstants.Context_Key_Result);
        }

        /// <summary>
        /// Checks if the endpoint has any problem set in HttpContext.Items.
        /// </summary>
        /// <param name="endpoint">The RadEndpoint instance</param>
        /// <returns>True if a problem is set, false otherwise</returns>
        public static bool HasProblem(this RadEndpoint endpoint)
        {
            return endpoint.HttpContext.Items.ContainsKey(RadConstants.Context_Key_RadProblem);
        }

        /// <summary>
        /// Gets the HTTP status code from any result type.
        /// </summary>
        /// <param name="endpoint">The RadEndpoint instance</param>
        /// <returns>The HTTP status code, or null if not determinable</returns>
        public static HttpStatusCode? GetStatusCode(this RadEndpoint endpoint)
        {
            var items = endpoint.HttpContext.Items;
            
            var result = items.TryGetValue(RadConstants.Context_Key_Result, out var resultValue) 
                ? resultValue as IResult 
                : null;

            if (result != null)
            {
                return result switch
                {   
                    RedirectHttpResult => HttpStatusCode.Found,
                    ForbidHttpResult => HttpStatusCode.Forbidden,
                    FileContentHttpResult => HttpStatusCode.OK,
                    FileStreamHttpResult => HttpStatusCode.OK,
                    PhysicalFileHttpResult => HttpStatusCode.OK,                    
                    IStatusCodeHttpResult statusCodeResult => (HttpStatusCode)statusCodeResult.StatusCode,
                    _ => null
                };
            }
            var problem = items.TryGetValue(RadConstants.Context_Key_RadProblem, out var problemValue) 
                ? problemValue 
                : null;

            if (problem != null)
            {
                return problem switch
                {
                    ProblemHttpResult problemHttpResult => (HttpStatusCode)problemHttpResult.StatusCode,
                    ValidationProblem validationProblem => (HttpStatusCode)validationProblem.StatusCode,
                    _ => null
                };
            }

            return null;
        }
    }
}