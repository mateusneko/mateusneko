using Microsoft.EntityFrameworkCore;
using StockPilot.Api.Errors;
using StockPilot.Core.Ports;
using StockPilot.Core.Services;
using StockPilot.Infrastructure.Data;
using StockPilot.Infrastructure.Repositories;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(o =>
    o.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));
builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<InventoryDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("Database") ?? "Data Source=stockpilot.db"));
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<InventoryService>();
var app = builder.Build();
app.UseExceptionHandler();
if (app.Environment.IsDevelopment()) app.MapOpenApi();
app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "healthy", app = "StockPilot" }));
using (var scope = app.Services.CreateScope())
    await scope.ServiceProvider.GetRequiredService<InventoryDbContext>().Database.EnsureCreatedAsync();
await app.RunAsync();
