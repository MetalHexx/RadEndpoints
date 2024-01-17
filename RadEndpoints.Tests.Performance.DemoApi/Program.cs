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

// This endpoint has some unnecessary parameters to equalize it with the implementation of RadEndpoints.
// Even though extremely small, additional parameters do add a small bit of overhead, so this is done to
// level the playing field in performance tests.
app.MapGet("/getusingminapi", ([AsParameters] DummyRequest r, IRadMediator _, HttpContext _, CancellationToken _) =>
{
    var response = new DummyResponse
    {
        Value = 1
    };

    return TypedResults.Ok(response);
});

app.Run();

public partial class Program;