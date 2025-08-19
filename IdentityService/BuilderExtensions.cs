using BuildingBlock.MassTransit;
using IdentityService.Data;
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

namespace IdentityService
{
    public static class BuilderExtensions
    {
        public static void AddIdentityService(this WebApplicationBuilder builder)
        {
            var asesembly = Assembly.GetExecutingAssembly();

            builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblies(asesembly); });

            builder.Services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(IdentityDbContext)), sql =>
            {
                sql.MigrationsHistoryTable("__EFMigrationsHistory", ServiceConstants.NAME);
            }));

            builder.AddCustomMassTransit(asesembly);

            builder.Services.Configure<InternalServices>(builder.Configuration.GetSection(nameof(InternalServices)));
            builder.Services.Configure<StaticParams>(builder.Configuration.GetSection(nameof(StaticParams)));
        }
    }
}
