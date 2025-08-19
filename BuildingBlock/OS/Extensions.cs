using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.OS
{
    public static class Extensions
    {
        public static void AddCustomOs(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        }
    }
}
