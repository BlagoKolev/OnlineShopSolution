using OnlineShop.Models.Dtos;

namespace OnlineShop.Web.Services.Contracts
{
    public interface IShoppingCartService
    {
        Task<IEnumerable<CartItemDto>> GetItems(int userId);
        Task<CartItemDto> AddItem(CartItemToAddDto itemToAdd);
    }
}
