using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using simple_authentication_client_infrastructure;
using simple_authentication_client_infrastructure.Context;
using simple_authentication_client_integration_tests.Implementations;

namespace simple_authentication_client_integration_tests.Base;

public class TestBase
{
    private int UsersCount => TestContext.CurrentContext.Test.Properties.ContainsKey("UsersCount")
        ? Int32.Parse(TestContext.CurrentContext.Test.Properties["UsersCount"].ElementAt(0).ToString() ?? "10")
        : 10;
    private UnitTestWebApplicationFactory app = null!;
    protected static string User1 = "user1";

    protected HttpClient HttpClient = null!;

    public async Task Initialize()
    {
        Environment.SetEnvironmentVariable("IS_INTEGRATION_TEST_ENVIRONMENT", "true");
        app = new UnitTestWebApplicationFactory();
        await app.InitializeAsync();
        HttpClient = app.CreateClient();

        var scopeFactory = app.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TemplateAppContext>();
        await context.Database.MigrateAsync();
        await ResetState(scope);
    }

    public async Task StopAsync()
    {
        await app.StopAsync();
    }

    public async Task ResetState(IServiceScope scope)
    {
        var context = scope.ServiceProvider.GetRequiredService<TemplateAppContext>();

        var seedData = new SeedDataHelper(context);

        for (int i = 0; i < UsersCount; i++)
        {
            await seedData.AddUser($"user{i}", i * 11);
        }
    }
}