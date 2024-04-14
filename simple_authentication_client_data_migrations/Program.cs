using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using simple_authentication_client_infrastructure.Context;

namespace simple_authentication_client_migrations
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables();
            var configuration = builder.Build();

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {
                    builder.Sources.Clear();
                    builder.AddConfiguration(configuration);
                })
                .ConfigureServices((hostBuilder, services) =>
                {
                    services.AddTransient<SeedDataHelper>();
                    var connectionString = hostBuilder.Configuration.GetConnectionString("default");
                    services.AddDbContext<TemplateAppContext>(options => options.UseSqlServer(connectionString));
                })
                .Build();

            var context = host.Services.GetRequiredService<TemplateAppContext>();
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await context.Database.EnsureCreatedAsync();

                var clearScript = @"DELETE ""dbo"".""UserSession"";
                                    DELETE ""dbo"".""User"";";
                await context.Database.ExecuteSqlRawAsync(clearScript);

                var seedData = host.Services.GetRequiredService<SeedDataHelper>();

                await seedData.AddUser("User1", 11);
                await seedData.AddUser("User2", 22);
                await seedData.AddUser("User3", 33);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
            }
        }
    }
}
