using user_api_csharp.src.Data;
using Microsoft.EntityFrameworkCore;

namespace user_api_csharp.src.Extensions;

public static class WebApplicationExtensions
{
  public static WebApplication ApplyMigrations(this WebApplication app)
  {
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();

    return app;
  }

  public static WebApplication UseApiPipeline(this WebApplication app)
  {
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

    return app;
  }
}
