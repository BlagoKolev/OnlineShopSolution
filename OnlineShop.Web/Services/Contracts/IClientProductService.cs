using OnlineShop.Models.Dtos;

namespace OnlineShop.Web.Services.Contracts
{
    public interface IClientProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
