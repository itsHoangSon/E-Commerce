using Serilog;
using Serilog.Enrichers.Span;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Logging
{
    public static class Extensions
    {
        public static void AddCustomLogging(this WebApplicationBuilder builder)
        {



            builder.Host.UseSerilog((context, loggerConfiguration) =>
            {

                loggerConfiguration
                    //.WriteTo.Console()
                    //.WriteTo.File("logs/logs.txt", rollingInterval: RollingInterval.Day)
                    //.MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
                    //.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    //.MinimumLevel.Override("Hangfire", LogEventLevel.Error)

                    .Enrich.WithSpan()
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("ServiceName", AppDomain.CurrentDomain.FriendlyName)
                    .ReadFrom.Configuration(context.Configuration);
            });
            builder.Services.AddSingleton(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            // Add log behavior
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));


            //builder.Services.AddTransient<DefaultRequestIdMessageHandler>();
            //builder.Services.AddScoped<ISessionIdAccessor, DefaultSessionIdAccessor>();


        }
    }
}
