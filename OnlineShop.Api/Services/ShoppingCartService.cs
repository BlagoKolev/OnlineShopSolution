using Microsoft.EntityFrameworkCore;
using OnlineShop.Api.Services.Contracts;
using OnlineShop.Data.Context;
using OnlineShop.Data.Models;
using OnlineShop.Models.Dtos;

namespace OnlineShop.Api.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly OnlineShopDbContext db;

        public ShoppingCartService(OnlineShopDbContext db)
        {
            this.db = db;
        }

        private async Task<bool> CartItemExists(int cartId, int productId)
        {
            return await db.CartItems.AnyAsync(x => x.CartId == cartId && x.ProductId == productId);
        }
        public async Task<CartItemDto> AddProduct(CartItemToAddDto cartItemToAddDto)
        {
            if (await CartItemExists(cartItemToAddDto.CartId, cartItemToAddDto.ProductId) == false)
            {
                var productToAdd = await db.Products
               .Where(x => x.Id == cartItemToAddDto.ProductId)
               .Select(x => new CartItem
               {
                   CartId = cartItemToAddDto.CartId,
                   ProductId = x.Id,
                   Quantity = cartItemToAddDto.Quantity,
               })
               .FirstOrDefaultAsync();

                var realProduct = db.Products
                    .Where(x => x.Id == productToAdd.ProductId)
                    .FirstOrDefault();

                productToAdd.Product = realProduct;

                if (productToAdd != null)
                {
                    var result = await db.CartItems.AddAsync(productToAdd);
                    await db.SaveChangesAsync();
                    var itemToReturn = new CartItemDto
                    {
                        Id = productToAdd.Id,
                        CartId = productToAdd.CartId,
                        Price = productToAdd.Product.Price,
                        ProductDescription = productToAdd.Product.Description,
                        ProductId = productToAdd.ProductId,
                        ProductImageUrl = productToAdd.Product.ImageUrl,
                        ProductName = productToAdd.Product.Name,
                        Quantity = productToAdd.Quantity,
                        TotalPrice = productToAdd.Quantity * productToAdd.Product.Price
                    };
                    //return result.Entity;
                    return itemToReturn;
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

            //var cartItemDto = new CartItemDto
            //{
            //    Id = product.Id,
            //    ProductName = product.Product.Name,
            //    ProductDescription = product.Product.Description,
            //    ProductImageUrl = product.Product.ImageUrl,
            //    ProductId = product.ProductId,
            //    Price = product.Product.Price,
            //    Quantity = product.Quantity,
            //    TotalPrice = product.Product.Price * product.Quantity,
            //    CartId = product.CartId,
            //};
            var cartItemDto = ConvertToDto(product);

            return cartItemDto;
        }

        public async Task<CartItemDto> RemoveFromShopingCart(int id)
        {
            var itemToRemove = await db.CartItems
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            var product = db.Products
                .Where(x => x.Id == itemToRemove.ProductId)
                .FirstOrDefault();

            itemToRemove.Product = product;

            if (itemToRemove == null)
            {
                return null;
            }

            db.CartItems.Remove(itemToRemove);
            await db.SaveChangesAsync();

            var itemtoRemoveDto = ConvertToDto(itemToRemove);
            return itemtoRemoveDto;
        }

        public async Task<CartItemDto> UpdateQuantity(int id, CartItemUpdateQuantityDto cartItemUpdateQuantityDto)
        {
            var itemToUpdate = await db.CartItems
                 .Where(x => x.Id == id)
                 //.Select(x=> new CartItem
                 //{
                 //    Id = x.Id,
                 //    ProductId = x.ProductId,
                 //    Product = x.Product,
                 //    CartId = x.CartId,
                 //    Quantity = x.Quantity
                 //})
                 .FirstOrDefaultAsync();

            var product = db.Products
               .Where(x => x.Id == itemToUpdate.ProductId)
               .FirstOrDefault();

            itemToUpdate.Product = product;


            if (itemToUpdate != null)
            {
                itemToUpdate.Quantity = cartItemUpdateQuantityDto.Quantity;
               var res = await db.SaveChangesAsync();
                var itemToReturn = ConvertToDto(itemToUpdate);
                return itemToReturn;
            }
            return null;
        }

        private static CartItemDto ConvertToDto(CartItem cartItem)
        {
            var cartItemDto = new CartItemDto
            {
                Id = cartItem.Id,
                ProductName = cartItem.Product.Name,
                ProductDescription = cartItem.Product.Description,
                ProductImageUrl = cartItem.Product.ImageUrl,
                ProductId = cartItem.ProductId,
                Price = cartItem.Product.Price,
                Quantity = cartItem.Quantity,
                TotalPrice = cartItem.Product.Price * cartItem.Quantity,
                CartId = cartItem.CartId,
            };
            return cartItemDto;
        }
    }
}
