using Microsoft.AspNetCore.Components;
using OnlineShop.Models.Dtos;
using OnlineShop.Web.Services.Contracts;

namespace OnlineShop.Web.Pages
{
    public class ShoppingCartBase : ComponentBase
    {
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        public List<CartItemDto> ShoppingCartItems { get; set; }
        public string? ErrorMessage { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                 ShoppingCartItems = await this.ShoppingCartService.GetItems(HardCoded.UserId);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task DeleteItemFromCartClick(int id)     
        {
            var cartItemDto = await this.ShoppingCartService.DeleteItem(id);
            RemoveCartItem(id);
        }
        private CartItemDto GetCartItem(int id)
        {
           return this.ShoppingCartItems.FirstOrDefault(x => x.Id == id);
        }

        private void RemoveCartItem(int id)
        {
            var itemToRemove = GetCartItem(id);
            ShoppingCartItems.Remove(itemToRemove);
        }

        protected async Task UpdateQuantityClick(int id, int quantity)
        {
            try
            {
                if (quantity > 0)
                {
                    var updateItemDto = new CartItemUpdateQuantityDto
                    {
                        CartItemId = id,
                        Quantity = quantity
                    };
                   var returnedUpdateItemDto = await ShoppingCartService.UpdateItemQuantity(updateItemDto);
                }
                else
                {
                    var item = this.ShoppingCartItems.FirstOrDefault(x=>x.Id == id);

                    if (item != null)
                    {
                        item.Quantity = 1;
                        item.TotalPrice = item.Price;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
