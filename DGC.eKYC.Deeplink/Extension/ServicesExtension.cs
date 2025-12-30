using Azure.Core.Serialization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace DGC.eKYC.Deeplink.Extension;

public static class ServicesExtension
{
    public static void AddDefault(this IServiceCollection services)
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        var defaultJsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Use camelCase for JSON properties
            WriteIndented = true,                              // Pretty print the JSON
            AllowTrailingCommas = true,                        // Allow trailing commas during deserialization
        };

        var defaultJsonObjSerDeOpt = new JsonObjectSerializer(defaultJsonSerializerOptions);

        services.AddSingleton(defaultJsonObjSerDeOpt);
    }
}