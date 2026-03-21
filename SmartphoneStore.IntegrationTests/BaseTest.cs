using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using SmartphoneStore.Dal;

namespace SmartphoneStore.IntegrationTests;

public class BaseTest : IDisposable
{
    private IHostBuilder _server;
    private IHost _host;
    protected SqlDbContext SqlDbContext;

    public HttpClient GetClient()
    {
        _host = _server.Start();
        SqlDbContext = _host.Services.GetRequiredService<SqlDbContext>();
        return _host.GetTestClient();
    }

    protected BaseTest InitTestServer()
    {
        _server = new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder.UseTestServer();
                webBuilder.UseStartup<TestStartup>();
            });

        return this;
    }

    public void Dispose()
    {
        _host.StopAsync().GetAwaiter().GetResult();
        _host.Dispose();
        SqlDbContext.Dispose();
    }
}