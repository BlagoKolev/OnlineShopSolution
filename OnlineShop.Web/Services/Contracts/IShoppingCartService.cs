using OnlineShop.Models.Dtos;

namespace OnlineShop.Web.Services.Contracts
{
    public interface IShoppingCartService
    {
        Task<List<CartItemDto>> GetItems(int userId);
        Task<CartItemDto> AddItem(CartItemToAddDto itemToAdd);
        Task<CartItemDto> DeleteItem(int id);
        Task<CartItemDto> UpdateItemQuantity(CartItemUpdateQuantityDto cartItemUpdateQuantityDto);
    }
}
