using AutoMapper;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using SmartphoneStore.Api.Smartphone;
using SmartphoneStore.Api.Tablet;
using SmartphoneStore.Dal;
using SmartphoneStore.Dal.Smartphone;
using SmartphoneStore.Dal.Tablet;
using SmartphoneStore.Model.MessageBroker;
using SmartphoneStore.Model.Smartphone;
using SmartphoneStore.Model.Tablet;
using SmartphoneStore.Model.TabletSmartphone;
using SmartphoneStore.Orchestrator.Smartphone;
using SmartphoneStore.Orchestrator.Tablet;
using SmartphoneStore.Orchestrator.TabletSmartphone;
using SmartphoneStore.Platform.BlobStorage;
using SmartphoneStore.Platform.MessageBroker;

namespace SmartphoneStore.Api;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddScoped<ISmartphoneOrchestrator, SmartphoneOrchestrator>();
        services.AddScoped<ISmartphoneRepository, SmartphoneRepository>();
        services.AddScoped<ITabletOrchestrator, TabletOrchestrator>();
        services.AddScoped<ITabletRepository, TabletRepository>();
        services.AddScoped<ITabletSmartphoneOrchestrator, TabletSmartphoneOrchestrator>();

        services.AddAutoMapper(config => config.AddProfiles(
            new List<Profile>
            {
                new SmartphoneMap(),
                new DaoMap(),
                new TabletMap()
            }));

        ConfigureDb(services);
        ConfigureEdgeServices(services);
    }

    protected virtual void ConfigureDb(IServiceCollection services)
    {
        services.AddDbContext<SqlDbContext>(
            c => c.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

        services.AddDbContext<CosmosDbContext>(
            c => c.UseCosmos(_configuration.GetConnectionString("CosmosConnection"), "SmartphoneStoreNoSql"));
    }

    protected virtual void ConfigureEdgeServices(IServiceCollection services)
    {
        var blobConfig = new BlobConfiguration();

        _configuration.Bind("AzureBlobContainerConnectionString", blobConfig);
        services.AddSingleton(blobConfig);

        var client = new BlobServiceClient(blobConfig.ConnectionString);
        services.AddScoped<IBlobStorage, BlobStorage>();
        services.AddScoped(_ => client);

        services.AddSingleton(new ServiceBusClient(_configuration.GetConnectionString("ServiceBusConnectionString")));
        services.AddScoped<IPublisher, DeviceStatsPublisher>();

        var subscriberClient = new ServiceBusClient(_configuration.GetConnectionString("ServiceBusConnectionString"));
        var subscriber = new DeviceStatsSubscriber(subscriberClient);

        subscriber.SubscribeAsync().GetAwaiter().GetResult();

        services.AddSingleton<ISubscriber>(subscriber);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<Exception.GlobalExceptionMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}