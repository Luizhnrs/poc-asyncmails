using AsyncMails.Application;
using AsyncMails.Infrastructure;
using AsyncMails.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register layers via extension methods
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Auto-create database in development (use migrations in production)
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AsyncMailsDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

app.UseHttpsRedirection();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new
{
    Status = "Healthy",
    Timestamp = DateTime.UtcNow,
    Service = "AsyncMails.Api"
}))
.WithName("HealthCheck")
.WithTags("Health");

// Database connectivity check endpoint
app.MapGet("/health/db", async (AsyncMailsDbContext dbContext) =>
{
    try
    {
        var canConnect = await dbContext.Database.CanConnectAsync();
        return Results.Ok(new
        {
            Status = canConnect ? "Connected" : "Disconnected",
            Database = dbContext.Database.GetDbConnection().Database,
            Timestamp = DateTime.UtcNow
        });
    }
    catch (Exception ex)
    {
        return Results.Json(new
        {
            Status = "Error",
            Message = ex.Message,
            Timestamp = DateTime.UtcNow
        }, statusCode: 503);
    }
})
.WithName("DatabaseHealthCheck")
.WithTags("Health");

app.Run();
