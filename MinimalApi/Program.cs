using MinimalApi.Domain.Examples;
using MinimalApi.Features.CustomExamples.CustomPut;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRadEndpoints(typeof(Program));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<Program>(lifetime: ServiceLifetime.Scoped);
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IExampleService, ExampleService>();
builder.Services.AddSingleton<ICustomPutMapper, CustomPutMapper>();

var app = builder.Build();
app.MapRadEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Run();

public partial class Program { }