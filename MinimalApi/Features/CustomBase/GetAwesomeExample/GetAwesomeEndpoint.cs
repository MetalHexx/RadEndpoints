using MinimalApi.Features.CustomBase._common;

namespace MinimalApi.Features.CustomBase.GetAwesomeExample
{
    /// <summary>
    /// This endpoint uses a custom base endpoint to show how you can customize your own base endpoint for different applications and standards.
    /// </summary>
    public sealed class GetAwesomeEndpoint : AwesomeEndpoint<AwesomeRequest, AwesomeResponse>
    {
        public override void Configure()
        {
            Get("awesome")
                .WithDocument(tag: "Awesome Custom Base Endpoint", desc: "This endpoint uses a custom base endpoint to show how you can customize your own base endpoint for different applications and standards.");
        }

        public override async Task Handle(AwesomeRequest r, CancellationToken ct)
        {
            await Task.Delay(1, ct);
            SendString("Hello from a my awesome custom base endpoint!");
        }
    }
}
