using RadEndpoints;
using RadEndpoints.Tests.Performance.DemoApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddRadEndpoints(typeof(DummyRadEndpoint));

var app = builder.Build();

app.MapControllers();
app.MapRadEndpoints();

app.MapGet("/getusingminapi", () => 1);

app.Run();