using MinimalApi.Domain.Examples;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<Program>(lifetime: ServiceLifetime.Singleton); //This must be added before AddEndpoints to discover validators
builder.Services.AddEndpoints();
builder.Services.AddHttpContextAccessor();


builder.Services.AddSingleton<IExampleService, ExampleService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapEndpoints();

app.Run();
