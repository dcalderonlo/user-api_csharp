using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using user_api_csharp.src.Configuration;
using user_api_csharp.src.Services.Interfaces;
using user_api_csharp.src.Models;

namespace user_api_csharp.src.Services;

public class JwtTokenService(IOptions<JwtOptions> jwtOptions) : IJwtTokenService
{
  public AccessTokenResult CreateAccessToken(User user)
  {
    var options = jwtOptions.Value;
    var expiresAt = DateTime.UtcNow.AddMinutes(options.AccessTokenMinutes);

    var claims = new[]
    {
      new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
      new Claim(JwtRegisteredClaimNames.Email, user.Email),
      new Claim(JwtRegisteredClaimNames.UniqueName, user.Name),
      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key));
    var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
      issuer: options.Issuer,
      audience: options.Audience,
      claims: claims,
      expires: expiresAt,
      signingCredentials: credentials);

    var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
    return new AccessTokenResult(tokenValue, expiresAt);
  }
}
