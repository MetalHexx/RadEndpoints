using RadEndpoints;
using RadEndpoints.Mediator;
using RadEndpoints.Tests.Performance.DemoApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddRadEndpoints(typeof(DummyRadEndpoint));

var app = builder.Build();

app.MapControllers();
app.MapRadEndpoints();

app.MapGet("/getusingminapi", async ([AsParameters] DummyRequest r, IRadMediator m, HttpContext c, CancellationToken ct) =>
{
    // Simulate some artificial delay.
    await Task.Delay(TimeSpan.FromMilliseconds(5), ct);

    return new DummyResponse
    {
        Value = 1
    };
});

app.Run();

public partial class Program
{
}