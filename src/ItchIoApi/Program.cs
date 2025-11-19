using ItchIoApi.Models;
using ItchIoApi.Services;

namespace ItchIoApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure itch.io API settings
        builder.Services.Configure<ItchApiSettings>(
            builder.Configuration.GetSection(ItchApiSettings.SectionName));

        // Register HttpClient and ItchApiService
        builder.Services.AddHttpClient<IItchApiService, ItchApiService>();

        // Add services to the container.
        builder.Services.AddControllers();

        // Add health checks
        builder.Services.AddHealthChecks();

        // Add Swagger/OpenAPI documentation
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Title = "Itch.io API Wrapper",
                Version = "v1",
                Description = "ASP.NET Core API wrapper for interacting with the itch.io platform. " +
                             "Provides endpoints for managing games, users, purchases, and download keys."
            });

            // Add API key header parameter for Swagger UI
            options.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Description = "itch.io API key. Can be obtained from https://itch.io/user/settings/api-keys",
                Name = "X-API-Key",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "ApiKeyScheme"
            });

            // Include XML comments for better documentation
            var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Itch.io API v1");
                options.RoutePrefix = "swagger";
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        // Map health check endpoint
        app.MapHealthChecks("/health");

        app.Run();
    }
}
