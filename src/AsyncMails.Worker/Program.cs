using AsyncMails.Application;
using AsyncMails.Infrastructure;
using AsyncMails.Worker;

var builder = Host.CreateApplicationBuilder(args);

// Register layers via extension methods
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Register background workers
builder.Services.AddHostedService<NotificationWorker>();

var host = builder.Build();
host.Run();
