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
            SeedFakeUsers(services);
            SeedCategories(services);
            SeedProducts(services);
            SeedFakeCarts(services);

            return app;
        }

        private static void SeedFakeCarts(IServiceProvider services)
        {
            var db = GetDbContext(services);
            if (db.Carts.Any())
            {
                return;
            }
            using var openStream = File.OpenRead("../OnlineShop.Data/Resources/Carts.json");

            var carts = JsonSerializer
                .DeserializeAsync<Cart[]>(openStream)
                .GetAwaiter()
                .GetResult();
            db.Carts.AddRangeAsync(carts);
            db.SaveChangesAsync()
                .GetAwaiter()
                .GetResult();
        }
        private static void SeedFakeUsers(IServiceProvider services)
        {
            var db = GetDbContext(services);

            if (db.Users.Any())
            {
                return;
            }

            using FileStream openStream = File.OpenRead("../OnlineShop.Data/Resources/Users.json");
            var users = JsonSerializer
                .DeserializeAsync<ApplicationUser[]>(openStream)
                .GetAwaiter()
                .GetResult();

            db.Users
                .AddRangeAsync(users)
                .GetAwaiter()
                .GetResult();

            db.SaveChangesAsync()
                .GetAwaiter()
                .GetResult();
        }
        private static void SeedCategories(IServiceProvider services)
        {
            var db = GetDbContext(services);

            if (db.Categories.Any())
            {
                return;
            }

            using FileStream openStream = File.OpenRead("../OnlineShop.Data/Resources/Categories.json");
            var categories = JsonSerializer
                .DeserializeAsync<Category[]>(openStream)
                .GetAwaiter()
                .GetResult();

            db.Categories.AddRangeAsync(categories)
                .GetAwaiter()
                .GetResult();
            db.SaveChangesAsync()
                .GetAwaiter()
                .GetResult();
        }
        private static void SeedProducts(IServiceProvider services)
        {
            var db = GetDbContext(services);

            if (db.Products.Any())
            {
                return;
            }

            using FileStream openStream = File.OpenRead("../OnlineShop.Data/Resources/Products.json");
            var products = JsonSerializer
                .DeserializeAsync<Product[]>(openStream)
                .GetAwaiter()
                .GetResult();

            db.Products.AddRangeAsync(products)
                .GetAwaiter()
                .GetResult();

            db.SaveChangesAsync()
                .GetAwaiter()
                .GetResult();
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
