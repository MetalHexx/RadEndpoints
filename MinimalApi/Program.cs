using MinimalApi.Domain.Examples;
using MinimalApi.Features.WithTypedResults.TypedResultsPut;
using MinimalApi.Features.Pure.UpdateExample;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRadEndpoints(typeof(Program));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IExampleService, ExampleService>();
builder.Services.AddSingleton<ICustomPutMapper, TypedResultsPutMapper>();
                
var app = builder.Build();
app.MapRadEndpoints();
app.MapPureEndpoints();
//app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Run();

public partial class Program { }