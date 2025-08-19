
using BuildingBlock;
using E_Commerce;

namespace ECommerceService.WebApi
{
    public static class BuilderExtensions
    {
        public static void AddAppServices(this WebApplicationBuilder builder)
        {
            builder.AddBuildingBlocks();

            builder.AddECommerceService();

            builder.Services.AddControllers();

        }
        public static void UseAppPipeline(this WebApplication app)
        {
            app.UseBuildingBlocks();
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

        }
    }
}
