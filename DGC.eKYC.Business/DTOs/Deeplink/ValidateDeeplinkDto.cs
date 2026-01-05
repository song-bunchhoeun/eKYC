using System.ComponentModel.DataAnnotations;
using DGC.eKYC.Business.DTOs.SuperAppSecurity;
using System.Text.Json.Serialization;
using DGC.eKYC.Business.Validations.ModelState;

namespace DGC.eKYC.Business.DTOs.Deeplink;

public class ValidateDeeplinkInputDto : SuperAppSecurityBaseInput
{
    [Required]
    [JsonPropertyName("deeplinkId")]
    public string DeeplinkRequestId { get; set; }

    [StringLength(64, ErrorMessage = "Dealer Id cannot be longer than 64 characters. Please contact administrator!")]
    public string DealerId { get; set; }

    [CambodianPhoneNumberValidation]
    public string PhoneNumber { get; set; }

    [UnixTimeSecondsValidation]
    public long Timestamp { get; set; }

    [Required]
    public int OrgId { get; set; }

    [CallBackUrlValidation]
    public string CallBackUrl { get; set; }
}

public class ValidateDeeplinkOutputDto(string token)
{
    public string Token { get; set; } = token;
}