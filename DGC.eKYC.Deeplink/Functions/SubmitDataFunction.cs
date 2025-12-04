using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DGC.eKYC.Deeplink.Functions;

public class SubmitDataFunction
{
    private readonly ILogger _logger;

    public SubmitDataFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<SubmitDataFunction>();
    }

    [Function("SubmitDataFunction")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
        FunctionContext ctx)   // add FunctionContext to access context.Items
    {
        _logger.LogInformation("POST request received.");

        // version injected by middleware
        var version = ctx.Items.ContainsKey("ApiVersion") ? ctx.Items["ApiVersion"]?.ToString() : null;

        var body = await req.ReadAsStringAsync();

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteStringAsync($"Received: {body}\nAPI-Version: {version}");

        return response;
    }
}
