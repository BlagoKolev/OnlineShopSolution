using Microsoft.AspNetCore.Components;
using OnlineShop.Models.Dtos;
using OnlineShop.Web.Services.Contracts;

namespace OnlineShop.Web.Pages
{
    public class ProductsBase : ComponentBase
    {
        [Inject]
        public IProductService? ProductService { get; set; }
        [Inject]
        public IShoppingCartService? ShoppingCartService { get; set; }

        public IEnumerable<ProductDto>? Products { get; set; }
        public string ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Products = await ProductService.GetProducts();
                var shoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);

                var totalQuantity = shoppingCartItems.Sum(x => x.Quantity);

                ShoppingCartService.RaiseEventOnShoppingCartChanged(totalQuantity);


            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
        protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
        {
            return from product in Products
                   group product by product.CategoryId into groupedProducts
                   orderby groupedProducts.Key
                   select groupedProducts;
        }

        protected string GetCategoryName(IGrouping<int, ProductDto> groupedProductsDto)
        {
            return groupedProductsDto
                .FirstOrDefault(x => x.CategoryId == groupedProductsDto.Key)
                .CategoryName;
        }
    }
}
