using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace RadEndpoints
{
    public abstract partial class RadEndpoint
    {
        protected virtual IResult Send<TResponse>(TResponse responseData) => TypedResults.Ok(responseData);
        protected virtual IResult SendOk() => TypedResults.Ok();        
        protected virtual IResult SendNoContent() => TypedResults.NoContent();
        protected virtual IResult SendRedirect(string url, bool permanent = false, bool preserveMethod = false) => TypedResults.Redirect(url, permanent, preserveMethod);
        protected virtual IResult SendToRoute(string? routeName = null, object? routeValues = null, bool permanent = false, bool preserveMethod = false, string? fragment = null) => 
            TypedResults.RedirectToRoute(routeName, routeValues, permanent, preserveMethod, fragment);
        protected virtual IResult SendString<TResponse>(string body) => TypedResults.Ok(body);
        protected virtual IResult SendCreatedAt(string uri) => TypedResults.Created(uri);
        protected virtual IResult SendCreatedAt<TResponse>(string uri, TResponse response) => TypedResults.Created(uri, response);
        protected virtual IResult SendInternalError(string title) => TypedResults.Problem(title: title, statusCode: StatusCodes.Status500InternalServerError);
        protected virtual IResult SendExternalError(string title) => TypedResults.Problem(title: title, statusCode: StatusCodes.Status502BadGateway);
        protected virtual IResult SendExternalTimeout(string title) => TypedResults.Problem(title: title, statusCode: StatusCodes.Status504GatewayTimeout);
        protected virtual IResult SendValidationError(string title) => TypedResults.Problem(title: title, statusCode: StatusCodes.Status400BadRequest);
        protected virtual IResult SendConflict(string title) => TypedResults.Problem(title: title, statusCode: StatusCodes.Status409Conflict);
        protected virtual IResult SendNotFound(string title) => TypedResults.Problem(title: title, statusCode: StatusCodes.Status404NotFound);
        protected virtual IResult SendUnauthorized(string title) => TypedResults.Problem(title: title, statusCode: StatusCodes.Status401Unauthorized);
        protected virtual IResult SendForbidden(string title) => TypedResults.Problem(title: title, statusCode: StatusCodes.Status403Forbidden);
        protected virtual IResult SendBytes(byte[] contents, string? contentType = null, string? fileDownloadName = null, bool enableRangeProcessing = false, DateTimeOffset? lastModified = null) => 
            TypedResults.Bytes(contents, contentType, fileDownloadName, enableRangeProcessing, lastModified);
        protected virtual IResult SendFile(byte[] fileContents, string? contentType = null, string? fileDownloadName = null, bool enableRangeProcessing = false, DateTimeOffset? lastModified = null, EntityTagHeaderValue? entityTag = null) => 
            TypedResults.File(fileContents, contentType, fileDownloadName, enableRangeProcessing, lastModified, entityTag);
        protected virtual IResult SendStream(Stream stream, string? contentType = null, string? fileDownloadName = null, DateTimeOffset? lastModified = null, EntityTagHeaderValue? entityTag = null, bool enableRangeProcessing = false) => 
            TypedResults.Stream(stream, contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing);

    }
}
