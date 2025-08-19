using BuildingBlock.Exceptions;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.MassTransit
{
    public static class Extensions
    {
        public static void AddCustomMassTransit(this WebApplicationBuilder builder, params Assembly[] assembly)
        {
            var rabbitMqSection = builder.Configuration.GetSection("RabbitMq");
            var rabbitMqOptions = rabbitMqSection.Get<RabbitMqOptions>();
            if (rabbitMqOptions == null) throw new CustomException(nameof(RabbitMqOptions));

            builder.Services.Configure<RabbitMqOptions>(rabbitMqSection);

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumers(assembly);
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqOptions.Host, rabbitMqOptions.Port, rabbitMqOptions.VirtualHost, h =>
                    {
                        h.Username(rabbitMqOptions.Username);
                        h.Password(rabbitMqOptions.Password);

                        //h.s

                    });

                    if (rabbitMqOptions.UseMessageRetry)
                    {
                        cfg.UseMessageRetry(r =>
                        {
                            r.Intervals(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(4), TimeSpan.FromSeconds(8), TimeSpan.FromSeconds(16));
                        });
                    }

                    cfg.ConfigureEndpoints(context);
                });

                x.SetEndpointNameFormatter(new DefaultEndpointNameFormatter("", true));
            });


        }
    }
}
