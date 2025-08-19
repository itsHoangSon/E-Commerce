using BuildingBlock.MassTransit;
using E_Commerce.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce
{
    public static class BuilderExtensions
    {
        public static void AddECommerceService(this WebApplicationBuilder builder)
        {
            var asesembly = Assembly.GetExecutingAssembly();

            builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblies(asesembly); });

            builder.Services.AddDbContext<ECommerceDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(ECommerceDbContext)), sql =>
            {
                sql.MigrationsHistoryTable("__EFMigrationsHistory", ServiceConstants.NAME);
            }));

            builder.AddCustomMassTransit(asesembly);

        }
    }
}
