﻿using Microsoft.AspNetCore.Components;
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
         protected IOrderedEnumerable<IGrouping<int,ProductDto>> GetGroupedProductsByCategory()
        {
            return from product in Products
                   group product by product.CategoryId into groupedProducts
                   orderby groupedProducts.Key
                   select groupedProducts;
        }

        protected string GetCategoryName(IGrouping<int,ProductDto> groupedProductsDto)
        {
            return groupedProductsDto
                .FirstOrDefault(x => x.CategoryId == groupedProductsDto.Key)
                .CategoryName;
        }
    }
}
