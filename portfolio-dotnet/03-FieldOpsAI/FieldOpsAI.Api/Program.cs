using FieldOpsAI.Api.Errors;
using FieldOpsAI.Core.Ports;
using FieldOpsAI.Core.Services;
using FieldOpsAI.Infrastructure.AI;
using FieldOpsAI.Infrastructure.Data;
using FieldOpsAI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(o =>
    o.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));
builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<IncidentsDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("Database") ?? "Data Source=fieldops.db"));
builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();
builder.Services.AddScoped<IncidentService>();
builder.Services.AddHttpClient("openai", c => c.Timeout = TimeSpan.FromSeconds(20));
builder.Services.AddScoped<ITriageEngine>(sp =>
{
    var rules = new RuleBasedTriageEngine();
    var key = builder.Configuration["OpenAI:ApiKey"];
    if (string.IsNullOrWhiteSpace(key)) return rules;
    var model = builder.Configuration["OpenAI:Model"] ?? "gpt-4.1-mini";
    var client = sp.GetRequiredService<IHttpClientFactory>().CreateClient("openai");
    return new FallbackTriageEngine(new OpenAiTriageEngine(client, key, model), rules);
});
var app = builder.Build();
app.UseExceptionHandler();
if (app.Environment.IsDevelopment()) app.MapOpenApi();
app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "healthy", app = "FieldOpsAI" }));
using (var scope = app.Services.CreateScope())
    await scope.ServiceProvider.GetRequiredService<IncidentsDbContext>().Database.EnsureCreatedAsync();
await app.RunAsync();
