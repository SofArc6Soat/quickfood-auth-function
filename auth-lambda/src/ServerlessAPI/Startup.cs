using Amazon.CognitoIdentityProvider;
using ServerlessAPI.Configurations;
using ServerlessAPI.Services;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ServerlessAPI
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });

            services.AddAWSService<IAmazonCognitoIdentityProvider>();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

            EnvironmentConfig.ConfigureEnvironment(services, _configuration);

            AddDependencies(services);
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void AddDependencies(IServiceCollection services)
        {
            services.AddScoped<IUsuarioService, UsuarioService>();

            services.AddScoped<ICognitoFactory, CognitoFactory>();
            services.AddScoped<ICognitoService, CognitoService>();
        }
    }
}
