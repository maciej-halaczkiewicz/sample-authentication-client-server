using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using simple_authentication_client_api;
using simple_authentication_client_application.Abstractions;
using Testcontainers.MsSql;

namespace simple_authentication_client_integration_tests.Implementations;

public class UnitTestWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly MsSqlContainer _dbContainer;
    private readonly int _port = Random.Shared.Next(10000, 15000);
    public UnitTestWebApplicationFactory()
    {
        _dbContainer = new MsSqlBuilder()
            .WithPortBinding(_port,
                1433)
            .WithName("mssql_" + _port)
            .Build();
        Thread.Sleep(100); // ensure container is warmed up
    }
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public async Task StopAsync()
    {
        await _dbContainer.StopAsync();
    }

    private void GetServices(IServiceCollection services)
    {
        services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
            w.EnvironmentName == "Development" &&
            w.ApplicationName == "simple_authentication_client_api"));
        services.AddSingleton<IDateTimeProvider, TestDateTimeProvider>();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((ctx, conf) =>
        {
            conf.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();
        });

        builder.ConfigureTestServices(GetServices);
        base.ConfigureWebHost(builder);
    }


    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(x =>
        {
            x.AddInMemoryCollection(new List<KeyValuePair<string, string?>>
            {
                new("ConnectionStrings:default",
                    _dbContainer.GetConnectionString()
                        .Replace("master", $"TemplateApp_{_port}")),
            });
        });

        return base.CreateHost(builder);
    }
}