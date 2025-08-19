
using ECommerceService.WebApi;

var builder = WebApplication.CreateBuilder(args);
builder.AddAppServices();

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowConfiguredOrigins", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("AllowConfiguredOrigins");
app.UseAppPipeline();
app.Run();