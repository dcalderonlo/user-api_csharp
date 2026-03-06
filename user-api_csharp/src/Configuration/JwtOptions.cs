using System.ComponentModel.DataAnnotations;

namespace user_api_csharp.src.Configuration;

public class JwtOptions
{
  public const string SectionName = "Jwt";

  [Required]
  public string Issuer { get; set; } = string.Empty;

  [Required]
  public string Audience { get; set; } = string.Empty;

  [Required]
  [MinLength(32)]
  public string Key { get; set; } = string.Empty;

  [Range(1, 1440)]
  public int AccessTokenMinutes { get; set; } = 15;

  [Range(1, 10080)]
  public int RefreshTokenMinutes { get; set; } = 120;
}
