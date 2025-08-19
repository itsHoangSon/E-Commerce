using BuildingBlock;

namespace IdentityService.WebApi
{
    public static class BuilderExtensions
    {
        public static void AddAppServices(this WebApplicationBuilder builder)
        {
            builder.AddBuildingBlocks();

            builder.AddIdentityService();

            //builder.Services.AddMemoryCache();

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
