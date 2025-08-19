using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.BackgroundTasks
{
    public static class Extensions
    {
        public static void AddCustomBackgroundTasks(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IBackgroundMediatRQueue, BackgroundMediatRQueue>();
            builder.Services.AddHostedService<QueuedHostedService>();
            builder.Services.AddSingleton<IBackgroundTaskQueue>(ctx =>
            {
                if (!int.TryParse(builder.Configuration["QueueCapacity"], out var queueCapacity))
                    queueCapacity = 100;
                return new BackgroundTaskQueue(queueCapacity);
            });
        }
    }
}
