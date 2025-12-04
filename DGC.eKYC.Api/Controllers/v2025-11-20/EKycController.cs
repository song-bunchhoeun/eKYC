using Asp.Versioning;
using DGC.eKYC.Business.DTOs.Errors;
using Microsoft.AspNetCore.Mvc;

namespace DGC.eKYC.Api.Controllers.v2025_11_20;

[ApiController]
[Route("api/ekyc")]
[ApiVersion("2025-11-20")]
[Produces("application/json")]
[Route("[controller]")]
public class EKycController(ILogger<EKycController> logger) : ControllerBase
{
    private readonly ILogger<EKycController> _logger = logger;

    /// <summary>
    /// Receives and updates the transaction status pushed from Acleda.
    /// </summary>
    /// <param name="topUpStatus">Transaction status details.</param>
    /// <param name="cancellationToken">cancellation token.</param>
    /// <returns>HTTP 200 OK with no response body.</returns>
    [HttpPost]
    //[ValidateCheckSum]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    [HttpGet("")]
    public async Task<IActionResult> TopUp([FromBody] object topUpInput, CancellationToken cancellationToken)
    {
        return Ok();
    }
}