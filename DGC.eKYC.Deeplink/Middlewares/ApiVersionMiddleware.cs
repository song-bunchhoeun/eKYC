using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace DGC.eKYC.Deeplink.Middlewares;

public class ApiVersionMiddleware : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        // Only care about HTTP-triggered functions
        var hasHttpTrigger = context.FunctionDefinition?.InputBindings
            .Any(b => b.Value.Type != null && b.Value.Type.EndsWith("HttpTrigger", StringComparison.OrdinalIgnoreCase)) ?? false;

        if (!hasHttpTrigger)
        {
            await next(context);
            return;
        }

        // Get HttpRequestData (null if something odd)
        var req = await context.GetHttpRequestDataAsync();
        if (req == null)
        {
            await next(context);
            return;
        }

        // Read query param "api-version"
        var version = System.Web.HttpUtility.ParseQueryString(req.Url.Query)["api-version"];

        if (!IsValidIsoDate(version))
        {
            var res = req.CreateResponse(HttpStatusCode.BadRequest);
            await res.WriteStringAsync("Invalid or missing api-version (expected yyyy-MM-dd).");
            // Short-circuit invocation with response
            context.GetInvocationResult().Value = res;
            return;
        }

        // Save version for handlers
        context.Items["ApiVersion"] = version;

        await next(context);
    }

    private bool IsValidIsoDate(string? v)
        => !string.IsNullOrEmpty(v) &&
           DateTime.TryParseExact(v, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _);
}