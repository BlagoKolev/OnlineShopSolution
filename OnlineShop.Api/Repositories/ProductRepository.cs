using Microsoft.EntityFrameworkCore;
using OnlineShop.Api.Repositories.Contracts;
using OnlineShop.Data.Context;
using OnlineShop.Data.Models;

namespace OnlineShop.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly OnlineShopDbContext db;

        public ProductRepository(OnlineShopDbContext db)
        {
            this.db = db;
        }
        public async Task<IEnumerable<Category>> GetCategories()
        {
            var categories = await db.Categories.ToArrayAsync();
            return categories;
        }

        public Task<Category> GetCategoryById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = await db.Products.ToArrayAsync();
            return products;
        }
    }
}
