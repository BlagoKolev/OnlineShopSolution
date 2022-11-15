using Microsoft.AspNetCore.Components;
using OnlineShop.Models.Dtos;
using OnlineShop.Web.Services.Contracts;

namespace OnlineShop.Web.Pages
{
    public class ProductDetailsBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IProductService clientProductService { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public ProductDto Product { get; set; }
        public string ErrorMessage { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                Product = await clientProductService.GetProductById(Id);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

        }

        protected async Task AddtoCartClick(CartItemToAddDto itemToAdd)
        {
            try
            {
                var cartItemDto = await ShoppingCartService.AddItem(itemToAdd);
                NavigationManager.NavigateTo("/ShoppingCart");
            }
            catch (Exception)
            {


            }
        }
    }
}
