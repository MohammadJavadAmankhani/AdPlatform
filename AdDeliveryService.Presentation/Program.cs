using Microsoft.Azure.Cosmos;
using AdDeliveryService.Core.Interfaces;
using AdDeliveryService.Infrastructure.Data;
using AdDeliveryService.Infrastructure.Messaging;
using MediatR;
using AdDeliveryService.Application.Queries;
using AdDeliveryService.Application.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
// Cosmos DB Configuration
builder.Services.AddSingleton<CosmosClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var cosmosConfig = config.GetSection("CosmosDb");

    var endpoint = cosmosConfig["AccountEndpoint"]
        ?? "https://localhost:8081/"; // Default emulator endpoint
    var key = cosmosConfig["AccountKey"]
        ?? "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="; // Default emulator key

    var client = new CosmosClient(
        endpoint,
        key,
        new CosmosClientOptions
        {
            ApplicationName = "AdDeliveryService",
            ConnectionMode = ConnectionMode.Gateway
        }
    );

    // Initialize with proper partition key
    InitializeCosmosDb(client, "AdsDb", "Ads", "/adId").Wait();
    return client;
});

async Task InitializeCosmosDb(CosmosClient client, string databaseName, string containerName, string partitionKeyPath)
{
    try
    {
        var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
        await database.Database.CreateContainerIfNotExistsAsync(new ContainerProperties
        {
            Id = containerName,
            PartitionKeyPath = partitionKeyPath
        });
        Console.WriteLine($"Initialized: {databaseName}/{containerName} with partition key {partitionKeyPath}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Initialization failed: {ex.Message}");
        throw;
    }
}

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetActiveAdsQuery).Assembly));


builder.Services.AddScoped<AdCreatedEventHandler>();

builder.Services.AddScoped<IAdDeliveryRepository, CosmosDbRepository>();
builder.Services.AddHostedService(sp =>
    new RabbitMQConsumer(builder.Configuration.GetConnectionString("RabbitMQ") ?? "amqp://guest:guest@localhost:5672/", sp));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

// Configure middleware
app.UseAuthorization();
app.MapControllers();

app.Run();