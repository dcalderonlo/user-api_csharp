using user_api_csharp.src.Extensions;

var builder = WebApplication.CreateBuilder(args);

var dbDirectory = Path.Combine(builder.Environment.ContentRootPath, "Db");
Directory.CreateDirectory(dbDirectory);

builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();
app.ApplyMigrations();
app.UseApiPipeline();

app.Run();
