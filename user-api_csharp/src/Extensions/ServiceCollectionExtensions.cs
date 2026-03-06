using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using user_api_csharp.src.Configuration;
using user_api_csharp.src.Data;
using user_api_csharp.src.Interfaces;
using user_api_csharp.src.Services;

namespace user_api_csharp.src.Extensions;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerWithJwt();
    services.AddJwtAuthentication(configuration);
    services.AddDatabase(configuration);
    services.AddApplicationDependencies();

    return services;
  }

  private static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
  {
    services.AddSwaggerGen(options =>
    {
      options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
      {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme. Example: Bearer {token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
      });

      options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
      {
        [new OpenApiSecuritySchemeReference("Bearer", doc, null)] = []
      });
    });

    return services;
  }

  private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
  {
    services
      .AddOptions<JwtOptions>()
      .Bind(configuration.GetSection(JwtOptions.SectionName))
      .ValidateDataAnnotations()
      .ValidateOnStart();

    var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
      ?? throw new InvalidOperationException("Jwt settings are not configured.");

    var keyBytes = Encoding.UTF8.GetBytes(jwtOptions.Key);

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options =>
      {
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
          ValidateIssuer = true,
          ValidIssuer = jwtOptions.Issuer,
          ValidateAudience = true,
          ValidAudience = jwtOptions.Audience,
          ValidateLifetime = true,
          ClockSkew = TimeSpan.Zero
        };
      });

    services.AddAuthorization();

    return services;
  }

  private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddDbContext<AppDbContext>(options =>
      options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

    return services;
  }

  private static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
  {
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<IPasswordHasher, Sha256PasswordHasher>();
    services.AddScoped<IRefreshTokenFactory, RefreshTokenFactory>();
    services.AddScoped<IJwtTokenService, JwtTokenService>();

    return services;
  }
}
