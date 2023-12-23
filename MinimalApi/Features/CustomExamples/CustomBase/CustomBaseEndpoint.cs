using MinimalApi.Features.CustomExamples._common;

namespace MinimalApi.Features.CustomExamples.CustomBase
{
    /// <summary>
    /// This endpoint uses a custom base endpoint to show how you can customize your own base endpoint for different applications and standards.
    /// </summary>
    public sealed class CustomBaseEndpoint : AwesomeEndpoint<CustomBaseRequest, CustomBaseResponse>
    {
        public override void Configure()
        {
            Get("custom-base")
                .WithDocument(tag: "Custom", desc: "This endpoint uses a custom base endpoint to show how you can customize your own base endpoint for different applications and standards.");
        }

        public override async Task Handle(CustomBaseRequest r, CancellationToken ct)
        {
            await Task.Delay(1, ct);
            SendString("Hello from a my awesome custom base endpoint!");
        }
    }

    public class CustomBaseRequest : RadRequest
    {
    }

    public class CustomBaseResponse : RadResponse
    {
    }
}
