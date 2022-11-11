using Microsoft.EntityFrameworkCore;
using OnlineShop.Api.Services.Contracts;
using OnlineShop.Data.Context;
using OnlineShop.Data.Models;
using OnlineShop.Models.Dtos;

namespace OnlineShop.Api.Services
{
    public class ShopingCartService : IShopingCartService
    {
        private readonly OnlineShopDbContext db;

        public ShopingCartService(OnlineShopDbContext db)
        {
            this.db = db;
        }

        private async Task<bool> CartItemExists(int cartId, int productId)
        {
            return await db.CartItems.AnyAsync(x => x.CartId == cartId && x.ProductId == productId);
        }
        public async Task<CartItem> AddProduct(CartItemToAddDto cartItemToAddDto)
        {
            if (await CartItemExists(cartItemToAddDto.CartId, cartItemToAddDto.ProductId) == false)
            {
                var productToAdd = await db.Products
               .Where(x => x.Id == cartItemToAddDto.ProductId)
               .Select(x => new CartItem
               {
                   CartId = cartItemToAddDto.CartId,
                   ProductId = x.Id,
                   Quantity = cartItemToAddDto.Quantity
               })
               .FirstOrDefaultAsync();

                if (productToAdd != null)
                {
                    var result = await db.CartItems.AddAsync(productToAdd);
                    await db.SaveChangesAsync();
                    return result.Entity;
                }
            }

            return null;
        }

        public async Task<IEnumerable<CartItemDto>> GetAllProductsInCart(int userId)
        {
            var productsInCart = await db.Carts
                .Where(cart => cart.UserId == userId)
              .Select(cart => cart.Items
              .Select(ci => new CartItemDto
              {
                  Id = ci.Id,
                  ProductId = ci.ProductId,
                  Quantity = ci.Quantity,
                  CartId = ci.CartId,
                  Price = ci.Product.Price,
                  ProductImageUrl = ci.Product.ImageUrl,
                  ProductDescription = ci.Product.Description,
                  ProductName = ci.Product.Name,
                  TotalPrice = ci.Product.Price * ci.Quantity,
              })
              .ToList())
              .FirstOrDefaultAsync();

            return productsInCart;
        }

        public async Task<CartItemDto> GetProductInCart(int productId, int cartId)
        {
            var product = await db.Carts
                 .Where(cart => cart.Id == cartId)
                 .Select(cart => cart.Items
                 .Where(item => item.Id == productId)
                 .Select(x => new CartItem
                 {
                     Id = x.Id,
                     ProductId = x.ProductId,
                     Quantity = x.Quantity,
                     CartId = x.CartId,

                 })
                 .FirstOrDefault())
                 .FirstOrDefaultAsync();

            var cartItemDto = new CartItemDto
            {
                Id = product.Id,
                ProductName = product.Product.Name,
                ProductDescription = product.Product.Description,
                ProductImageUrl = product.Product.ImageUrl,
                ProductId = product.ProductId,
                Price = product.Product.Price,
                Quantity = product.Quantity,
                TotalPrice = product.Product.Price * product.Quantity,
                CartId = product.CartId,
            };
            return cartItemDto;
        }

        public Task<CartItem> RemoveFromShopingCart(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CartItem> UpdateQuantity(int id, CartItemUpdateQuantityDto cartItemToAddDto)
        {
            throw new NotImplementedException();
        }
    }
}
