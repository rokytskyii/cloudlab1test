using Microsoft.Extensions.Configuration;
using EntityFrameworkCore.Testing.Common.Helpers;
using EntityFrameworkCore.Testing.Moq.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartphoneStore.Api;
using SmartphoneStore.Dal;

namespace SmartphoneStore.IntegrationTests;

public class TestStartup : Startup
{
    public TestStartup(IConfiguration configuration) : base(configuration)
    {
    }

    protected override void ConfigureDb(IServiceCollection services)
    {
        var context = ConfigureDb<SqlDbContext>();
        services.AddSingleton(context.MockedDbContext);
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