namespace RadEndpoints.Tests.Performance.DemoApi;

internal sealed class DummyRadEndpoint : RadEndpoint<DummyRequest, DummyResponse>
{
    public override void Configure()
    {
        Get("/getusingradendpoints");
    }

    public override async Task Handle(DummyRequest r, CancellationToken ct)
    {
        // Simulate some artificial delay.
        await Task.Delay(TimeSpan.FromMilliseconds(5), ct);
        
        Response.Value = 1;
    }
}