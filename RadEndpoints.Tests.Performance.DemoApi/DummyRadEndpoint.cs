namespace RadEndpoints.Tests.Performance.DemoApi;

internal sealed class DummyRadEndpoint : RadEndpoint<DummyRequest, DummyResponse>
{
    public override void Configure()
    {
        Get("/getusingradendpoints");
    }

    public override Task Handle(DummyRequest r, CancellationToken ct)
    {
        Response.Value = 1;

        return Task.CompletedTask;
    }
}