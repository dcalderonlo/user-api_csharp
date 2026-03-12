using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using user_api_csharp.src.Configuration;
using user_api_csharp.src.Data;
using user_api_csharp.src.Services;
using user_api_csharp.src.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var dbDirectory = Path.Combine(builder.Environment.ContentRootPath, "Db");
Directory.CreateDirectory(dbDirectory);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
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

builder.Services
	.AddOptions<JwtOptions>()
	.Bind(builder.Configuration.GetSection(JwtOptions.SectionName))
	.ValidateDataAnnotations()
	.ValidateOnStart();

var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
	?? throw new InvalidOperationException("Jwt settings are not configured.");

var keyBytes = Encoding.UTF8.GetBytes(jwtOptions.Key);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IRefreshTokenFactory, RefreshTokenFactory>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.ConfigObject.PersistAuthorization = true;
	});
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => Results.Ok(new
{
	message = "User API is running.",
	swagger = "/swagger",
	users = "/api/users"
}));

app.Run();
