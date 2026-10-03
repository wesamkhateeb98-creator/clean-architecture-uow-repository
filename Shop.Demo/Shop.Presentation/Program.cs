using Shop.Application.Extensions;
using Shop.Infrastructure.Extensions;
using Shop.Infrastructure.Persistence;
using Shop.Presentation.Extensions;
using Shop.Presentation.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Composition root: the ONLY place that knows every layer.
builder.Services
    .RegisterPresentation()
    .RegisterApplication()
    .RegisterInfrastructure(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

await DbInitializer.InitializeAsync(app.Services);

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapControllers();

app.Run();
