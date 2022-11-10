using OnlineShop.Data.Models;
using OnlineShop.Models.Dtos;

namespace OnlineShop.Api.Services.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
        Task<Product> GetProductById(int id);
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> GetCategoryById(int id);
    }
}
