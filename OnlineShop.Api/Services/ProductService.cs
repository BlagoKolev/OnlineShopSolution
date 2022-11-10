using Microsoft.EntityFrameworkCore;
using OnlineShop.Api.Services.Contracts;
using OnlineShop.Data.Context;
using OnlineShop.Data.Models;
using OnlineShop.Models.Dtos;

namespace OnlineShop.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly OnlineShopDbContext db;

        public ProductService(OnlineShopDbContext db)
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

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var products = await db.Products
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name
                })
                .ToArrayAsync();
            return products;
        }
    }
}
