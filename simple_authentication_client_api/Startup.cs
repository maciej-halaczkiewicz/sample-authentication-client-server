using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;
using simple_authentication_client_api.Implementations;
using simple_authentication_client_application;
using simple_authentication_client_application.Abstractions;
using simple_authentication_client_application.Common;
using simple_authentication_client_infrastructure;
using simple_authentication_client_infrastructure.Context;
using simple_authentication_client_infrastructure.Repositories;
using simple_authentication_client_infrastructure.UnitOfWork;

namespace simple_authentication_client_api
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CustomBearerTokenAuthSchemeHandler.AuthenticationScheme;
                    options.DefaultScheme = CustomBearerTokenAuthSchemeHandler.AuthenticationScheme;
                    options.DefaultChallengeScheme = CustomBearerTokenAuthSchemeHandler.AuthenticationScheme;
                })
                .AddScheme<CustomBearerTokenAuthSchemeOptions, CustomBearerTokenAuthSchemeHandler>(
                    CustomBearerTokenAuthSchemeHandler.AuthenticationScheme, options => { });

            simple_authentication_client_infrastructure.DependencyInjection.AddInfrastructure(services.AddApplication());

            var connectionString = _configuration.GetConnectionString("default");
            services.AddDbContext<TemplateAppContext>(options => options.UseSqlServer(connectionString));
            services.AddSingleton(Log.Logger);
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IClaimsTransformation, CustomClaimsTransformation>();
            services.AddScoped<IUserContext, UserContext>();

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(simple_authentication_client_infrastructure.DependencyInjection).Assembly);
            });
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddLogging();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("UserPolicy", policy =>
                {
                    policy.AddRequirements(new AssertionRequirement(context => context.User.Identity.Name.IsNotNullOrWhiteSpace()));
                });
            });
        }

        public void Configure(WebApplication app)
        {
            app.UseSerilogRequestLogging();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // could be enabled with docker ssh cert, otherwise redirection will not copy BEARER token in headers after redirection
            //app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            var IS_INTEGRATION_TEST_ENVIRONMENT = Environment.GetEnvironmentVariable("IS_INTEGRATION_TEST_ENVIRONMENT");

            if (IS_INTEGRATION_TEST_ENVIRONMENT != "true")
            {
                app.UseDefaultFiles(new DefaultFilesOptions
                {
                    DefaultFileNames = new List<string> { "index.html" }
                });
                app.UseStaticFiles();
            }

            app.MapControllers();
        }
    }
}