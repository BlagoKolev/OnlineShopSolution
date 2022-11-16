using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OnlineShop.Models.Dtos;
using OnlineShop.Web.Services.Contracts;

namespace OnlineShop.Web.Pages
{
    public class ShoppingCartBase : ComponentBase
    {
        [Inject]
        public IJSRuntime _IJSInteroop { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        public List<CartItemDto> ShoppingCartItems { get; set; }
        public string? ErrorMessage { get; set; }
        protected string TotalPrice { get; set; }
        protected int TotalQuantity { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await this.ShoppingCartService.GetItems(HardCoded.UserId);
                CartChanged();

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
            CartChanged();
        }
        private CartItemDto GetCartItem(int id)
        {
            return this.ShoppingCartItems.FirstOrDefault(x => x.Id == id);
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

                    UpdateItemTotalPrice(returnedUpdateItemDto);

                    CartChanged();

                    ToggleUpdateQuantityButton(returnedUpdateItemDto.Id, false);
                }
                else
                {
                    var item = this.ShoppingCartItems.FirstOrDefault(x => x.Id == id);

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

        protected async Task UpdateQuantityOnInput(int id)
        {
            await ToggleUpdateQuantityButton(id, true);
        }
        private async Task ToggleUpdateQuantityButton(int id, bool visible)
        {
            await _IJSInteroop.InvokeVoidAsync("ToggleUpdateQuantityButton", id, visible);
        }

        private void SetTotalPrice()
        {
            TotalPrice = this.ShoppingCartItems.Sum(x => x.TotalPrice).ToString("C");
        }
        private void SetTotalQuantity()
        {
            TotalQuantity = this.ShoppingCartItems.Sum(x => x.Quantity);
        }
        private void CalculateCartSummaryTotals()
        {
            SetTotalPrice();
            SetTotalQuantity();
        }
        private void UpdateItemTotalPrice(CartItemDto cartItemDto)
        {
            var item = GetCartItem(cartItemDto.Id);
            if (item != null)
            {
                item.TotalPrice = item.Quantity * item.Price;
            }
        }
        private void RemoveCartItem(int id)
        {
            var itemToRemove = GetCartItem(id);
            ShoppingCartItems.Remove(itemToRemove);
        }

        private void CartChanged()
        {
            CalculateCartSummaryTotals();
            ShoppingCartService.RaiseEventOnShoppingCartChanged(TotalQuantity);
        }
    }
}
