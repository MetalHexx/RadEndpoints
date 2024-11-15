using RadEndpoints;

namespace MinimalApi.Features.Redirect.WithPayload.RedirectWithPayloadEndpoint
{
    public class RedirectOrPayloadRequest 
    {
        public bool ShouldRedirect { get; set; }
    }

    public class RedirectOrPayloadResponse
    {
        public string Message { get; set; } = "Success!  Not redirected.";
    }
}