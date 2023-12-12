using MinimalApi.Domain.Examples;
using MinimalApi.Features.CustomExamples.CustomPut;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpoints();
builder.Services.AddValidatorsFromAssemblyContaining<Program>(lifetime: ServiceLifetime.Scoped);
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IExampleService, ExampleService>();
builder.Services.AddSingleton<ICustomPutMapper, CustomPutMapper>(); //To demonstrate how to create a custom mapper with no rad magic

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapEndpoints();
app.Run();

public partial class Program { }