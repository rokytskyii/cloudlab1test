using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartphoneStore.Api.Smartphone;
using SmartphoneStore.Dal;
using SmartphoneStore.Dal.Smartphone;
using SmartphoneStore.Model.Smartphone;
using SmartphoneStore.Orchestrator.Smartphone;

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

        services.AddAutoMapper(config => config.AddProfiles(
            new List<Profile>
            {
                new SmartphoneMap(),
                new DaoMap()
            }));

        ConfigureDb(services);
    }

    protected virtual void ConfigureDb(IServiceCollection services)
    {
        services.AddDbContext<SqlDbContext>(
            c => c.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));
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