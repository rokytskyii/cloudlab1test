using EntityFrameworkCore.Testing.Common.Helpers;
using EntityFrameworkCore.Testing.Moq.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SmartphoneStore.Api;
using SmartphoneStore.Dal;
using SmartphoneStore.Model.MessageBroker;
using SmartphoneStore.Platform.BlobStorage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartphoneStore.IntegrationTests;

public class TestStartup : Startup
{
    public static readonly List<string> MockedMessages = new();

    public TestStartup(IConfiguration configuration) : base(configuration)
    {
    }

    protected override void ConfigureDb(IServiceCollection services)
    {
        var sqlContext = ConfigureDb<SqlDbContext>();
        services.AddSingleton(sqlContext.MockedDbContext);

        var cosmosContext = ConfigureDb<CosmosDbContext>();
        services.AddSingleton(cosmosContext.MockedDbContext);
    }

    protected override void ConfigureEdgeServices(IServiceCollection services)
    {
        var blobConfig = new BlobConfiguration
        {
            ConnectionString = "UseDevelopmentStorage=true",
            ContainerName = "test-container"
        };
        services.AddSingleton(blobConfig);

        var mockBlobStorage = new Mock<IBlobStorage>();

        mockBlobStorage.Setup(x => x.ExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        mockBlobStorage.Setup(x => x.UploadBlobAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        mockBlobStorage.Setup(x => x.DeleteBlobAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        mockBlobStorage.Setup(x => x.GetAllFilesNameAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<int>());

        services.AddScoped<IBlobStorage>(_ => mockBlobStorage.Object);

        var mockPublisher = new Mock<IPublisher>();
        mockPublisher.Setup(x => x.PublishAsync(It.IsAny<string>()))
            .Callback<string>(msg => MockedMessages.Add(msg))
            .Returns(Task.CompletedTask);

        services.AddSingleton<IPublisher>(mockPublisher.Object);

        var mockSubscriber = new Mock<ISubscriber>();
        mockSubscriber.Setup(x => x.Data)
            .Returns(MockedMessages);

        mockSubscriber.Setup(x => x.SubscribeAsync())
            .Returns(Task.CompletedTask);

        services.AddSingleton<ISubscriber>(mockSubscriber.Object);
    }

    private IMockedDbContextBuilder<T> ConfigureDb<T>()
        where T : DbContext
    {
        var options = new DbContextOptionsBuilder<T>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = (T)Activator.CreateInstance(typeof(T), options);

        return new MockedDbContextBuilder<T>()
            .UseDbContext(context)
            .UseConstructorWithParameters(options);
    }
}