using Shop.Api.Middleware;
using Shop.Application;
using Shop.Infrastructure;
using Shop.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Composition root: the ONLY place that knows every layer.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("Default")!);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

await DbInitializer.InitializeAsync(app.Services);

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapControllers();

app.Run();
