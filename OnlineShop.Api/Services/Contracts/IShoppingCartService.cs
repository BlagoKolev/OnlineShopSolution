using OnlineShop.Data.Models;
using OnlineShop.Models.Dtos;

namespace OnlineShop.Api.Services.Contracts
{
    public interface IShoppingCartService
    {
        Task<CartItemDto> AddProduct(CartItemToAddDto cartItemToAddDto);
        Task<CartItem> UpdateQuantity(int id, CartItemUpdateQuantityDto cartItemToAddDto);
        Task<CartItemDto> RemoveFromShopingCart(int id);
        Task<CartItemDto> GetProductInCart(int productId, int cartId);
        Task<IEnumerable<CartItemDto>> GetAllProductsInCart(int userId);

    }
}
