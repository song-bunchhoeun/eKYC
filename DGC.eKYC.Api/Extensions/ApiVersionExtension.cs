using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using DGC.eKYC.Business.DTOs.Errors;
using Microsoft.AspNetCore.Mvc;

namespace DGC.eKYC.Api.Extensions;

public static class ApiVersionExtension
{
    public static void ConfigureAPI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = false; // No default API version
            options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
            options.ReportApiVersions = true; // Reports supported versions in response headers
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "yyyy-MM-dd"; // Format group name as YYYY-MM-DD
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    // Retrieve the environment from IConfiguration
                    var environment = configuration["ASPNETCORE_ENVIRONMENT"];

                    if (string.Equals(environment, "Production", StringComparison.OrdinalIgnoreCase))
                    {
                        // Hide details in production
                        return new BadRequestObjectResult(new ErrorResponse("bad_parameters", "needed parameters is not met for this API!", []));
                    }
                    else
                    {
                        // Show detailed validation errors in non-production environments
                        var errors = context.ModelState
                            .Where(ms => ms.Value?.Errors.Count > 0)
                            .Select(ms => new ErrorDetail(ms.Key, ms.Value?.Errors.FirstOrDefault()?.ErrorMessage.ToString(), []))
                            .ToList();

                        return new BadRequestObjectResult(new ErrorResponse("bad_request", "Validation failed for the request.", errors));
                    }
                };
            });
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            // Add a document for each API version
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = $"My API {description.GroupName}",
                    Version = description.GroupName.Replace("v", "")
                });
            }
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });
    }
}
