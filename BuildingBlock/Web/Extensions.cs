using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Web
{
    public static class Extensions
    {
        public static IHostBuilder ConfigureAppSettings(this IHostBuilder host)
        {
            host.ConfigureAppConfiguration((ctx, builder) =>
            {
                var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                const string root = "Config";
                //builder.SetBasePath("Config");
                builder.AddJsonFile($"{root}/appsettings.json", false, true);
                builder.AddJsonFile($"{root}/appsettings.{enviroment}.json", true, true);
                //builder.AddJsonFile($"appsettings.{Environment.MachineName}.json", true, true);

                builder.AddEnvironmentVariables();
            });

            return host;
        }

        public static void UsePathBase(this WebApplication app)
        {
            app.UsePathBase(new PathString(app.Configuration.GetValue<string>("PathBase")));
        }

        public static void AddCustomWeb(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        }

        //public static void SetMaxRequestBodySize(this WebApplicationBuilder builder, int maxRequestBodySize)
        //{
        //    builder.Services.Configure<IISServerOptions>(options =>
        //    {
        //        options.MaxRequestBodySize = maxRequestBodySize;
        //    });

        //    builder.Services.Configure<KestrelServerOptions>(options =>
        //    {
        //        options.Limits.MaxRequestBodySize = maxRequestBodySize; // if don't set default value is: 30 MB
        //    });

        //    builder.Services.Configure<FormOptions>(options =>
        //    {
        //        options.ValueLengthLimit = maxRequestBodySize;
        //        options.MultipartBodyLengthLimit = maxRequestBodySize; // if don't set default value is: 128 MB
        //        options.MultipartHeadersLengthLimit = maxRequestBodySize;
        //    });
        //}
    }
}
