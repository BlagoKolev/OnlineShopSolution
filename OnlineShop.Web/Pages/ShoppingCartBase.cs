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
                 ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task DeleteItemFromCartClick(int id)     
        {
            var cartItemDto = await ShoppingCartService.DeleteItem(id);
            RemoveCartItem(id);
        }
        private CartItemDto GetCartItem(int id)
        {
           return ShoppingCartItems.FirstOrDefault(x => x.Id == id);
        }

        private void RemoveCartItem(int id)
        {
            var itemToRemove = GetCartItem(id);
            ShoppingCartItems.Remove(itemToRemove);
        }
    }
}
