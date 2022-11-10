using Microsoft.AspNetCore.Mvc;
using OnlineShop.Api.Services.Contracts;
using OnlineShop.Models.Dtos;

namespace OnlineShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Get()
        {
            try
            {
                var productsDto = await productService.GetProducts();
               // var productCategories = await productService.GetCategories();

                if (productsDto == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(productsDto);
                }
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,"Error retrieving data drom database.");
            }
        }
    }
}
