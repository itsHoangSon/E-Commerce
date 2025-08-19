using BuildingBlock.Authorization;
using BuildingBlock.BackgroundTasks;
using BuildingBlock.Exceptions;
using BuildingBlock.Logging;
using BuildingBlock.OS;
using BuildingBlock.Swagger;
using BuildingBlock.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock
{
    public static class BuilderExtensions
    {
        public static void AddBuildingBlocks(this WebApplicationBuilder builder)
        {
            builder.Host.ConfigureAppSettings();

            builder.AddCustomWeb();
            builder.AddCustomAuthorization();
            builder.AddCustomLogging();
            builder.AddCustomOs();

            builder.AddCustomBackgroundTasks();
            builder.AddCustomSwagger();
        }

        public static void UseBuildingBlocks(this WebApplication app)
        {
            app.UseCustomSwagger();
            app.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}
