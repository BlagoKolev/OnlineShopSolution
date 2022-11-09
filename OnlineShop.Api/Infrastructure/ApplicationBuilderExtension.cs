using Microsoft.EntityFrameworkCore;
using OnlineShop.Data.Context;
using OnlineShop.Data.Models;
using System.Text.Json;

namespace OnlineShop.Api.Infrastructure
{
    public static class ApplicationBuilderExtension
    {
        public static async Task<IApplicationBuilder> PrepareDatabase(this IApplicationBuilder app)
        {
            var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);
            SeedCategories(services);
            SeedProducts(services);

            return app;
        }
        private static void SeedCategories(IServiceProvider services)
        {
            var db = GetDbContext(services);

            using FileStream openStream = File.OpenRead("../OnlineShop.Data/Resources/Categories.json");
            var categories = JsonSerializer.DeserializeAsync<Category[]>(openStream).GetAwaiter().GetResult(); 

            db.Categories.AddRangeAsync(categories).GetAwaiter().GetResult();
            db.SaveChangesAsync().GetAwaiter().GetResult();
        }
        private static void SeedProducts(IServiceProvider services)
        {
            var db = GetDbContext(services);

            if (db.Products.Any())
            {
                return;
            }

            using FileStream openStream = File.OpenRead("../OnlineShop.Data/Resources/Products.json");
            var products = JsonSerializer.DeserializeAsync<Product[]>(openStream).GetAwaiter().GetResult();

             db.Products.AddRangeAsync(products).GetAwaiter().GetResult();
            db.SaveChangesAsync().GetAwaiter().GetResult();
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
