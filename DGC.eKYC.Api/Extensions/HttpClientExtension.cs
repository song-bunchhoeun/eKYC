using Microsoft.Net.Http.Headers;
using Polly;
using Polly.Extensions.Http;

namespace DGC.eKYC.Api.Extensions;

public static class HttpClientExtension
{
    public static void ConfigureHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        var retryPolicySection = configuration.GetSection("AcledaApi:retryPolicy");
        var circuitBreakerSection = configuration.GetSection("AcledaApi:circuitBreakerPolicy");

        var maxRetries = retryPolicySection.GetValue<int>("maxRetries");
        var retryInterval = retryPolicySection.GetValue<int>("retryIntervalInSeconds");
        var treat404AsInactive = retryPolicySection.GetValue<bool>("treat404AsInactive");

        var failureThreshold = circuitBreakerSection.GetValue<int>("failureThreshold");
        var breakDuration = circuitBreakerSection.GetValue<int>("breakDurationInSeconds");

        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(r => treat404AsInactive && r.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(
                maxRetries,
                _ => TimeSpan.FromSeconds(retryInterval)
            );

        var circuitBreakerPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: failureThreshold,
                durationOfBreak: TimeSpan.FromSeconds(breakDuration)
            );

        //services.AddHttpClient("AcledaApi", httpClient =>
        //{
        //    httpClient.BaseAddress = new Uri(configuration["AcledaApi:BaseUrl"]!);

        //    httpClient.DefaultRequestHeaders.Add(
        //        HeaderNames.Accept, "application/json");
        //})
        //.AddPolicyHandler(retryPolicy)
        //.AddPolicyHandler(circuitBreakerPolicy);

        //services.AddHttpClient("FlowLogger", httpClient =>
        //{
        //    httpClient.BaseAddress = new Uri(configuration["FlowLogger:BaseUrl"]!);

        //    httpClient.DefaultRequestHeaders.Add(
        //        HeaderNames.Accept, "application/json");
        //})
        //.AddPolicyHandler(retryPolicy)
        //.AddPolicyHandler(circuitBreakerPolicy);
    }
}
