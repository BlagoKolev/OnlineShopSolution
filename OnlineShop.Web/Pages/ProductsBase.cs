using Microsoft.AspNetCore.Components;
using OnlineShop.Models.Dtos;
using OnlineShop.Web.Services.Contracts;

namespace OnlineShop.Web.Pages
{
    public class ProductsBase : ComponentBase
    {
        [Inject]
        public IClientProductService? IClientProductService{ get; set; }

        public IEnumerable<ProductDto>? Products { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Products = await IClientProductService.GetProducts();
        }


    }
}
