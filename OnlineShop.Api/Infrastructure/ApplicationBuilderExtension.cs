using Microsoft.EntityFrameworkCore;
using OnlineShop.Data.Context;

namespace OnlineShop.Api.Infrastructure
{
    public static class ApplicationBuilderExtension
    {
        public static async Task<IApplicationBuilder> PrepareDatabase(this IApplicationBuilder app)
        {
            var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var db = GetDbContext(services);
            db.Database.Migrate();
        }
        private static OnlineShopDbContext GetDbContext(IServiceProvider services)
        {
            return services.GetRequiredService<OnlineShopDbContext>();

        }
    }
}
