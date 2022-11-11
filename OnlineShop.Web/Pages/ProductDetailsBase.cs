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
        public IClientProductService clientProductService { get; set; }

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
    }
}
