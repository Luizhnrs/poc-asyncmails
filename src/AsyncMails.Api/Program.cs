using AsyncMails.Application;
using AsyncMails.Application.DTOs;
using AsyncMails.Application.Services;
using AsyncMails.Infrastructure;
using AsyncMails.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register layers via extension methods
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Auto-create database in development (use migrations in production)
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AsyncMailsDbContext>();
    await dbContext.Database.EnsureCreatedAsync();

    // Enable Swagger
    app.UseSwagger();
    app.UseSwaggerUI();
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

// Notifications endpoint
app.MapPost("/notifications", async (CreateNotificationRequest request, INotificationService notificationService, CancellationToken cancellationToken) =>
{
    var result = await notificationService.ProcessCreateNotificationAsync(request, cancellationToken);
    return Results.Ok(result);
})
.WithName("CreateNotification")
.WithTags("Notifications");

app.Run();
