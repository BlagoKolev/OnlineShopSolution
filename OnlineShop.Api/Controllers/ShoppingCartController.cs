using Microsoft.AspNetCore.Mvc;
using OnlineShop.Api.Services.Contracts;
using OnlineShop.Models.Dtos;

namespace OnlineShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IShopingCartService shopingCartService;

        public ShoppingCartController(IProductService productService,
            IShopingCartService shopingCartService)
        {
            this.productService = productService;
            this.shopingCartService = shopingCartService;
        }

        [HttpGet]
        [Route("{userId}/GetProducts")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetProducts(int userId)
        {
            try
            {
                var productsInCart = await shopingCartService.GetAllProductsInCart(userId);
                if (productsInCart == null)
                {
                    return NoContent();
                }

                var products = await productService.GetProducts();
                if (products == null)
                {
                    throw new Exception("No products exist at all at database.");

                }
                return Ok(productsInCart);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<CartItemDto>> GetCartProductById(int productId, int cartId)
        {
            try
            {
                var currentProduct = await shopingCartService.GetProductInCart(productId, cartId);
                var product = await productService.GetProductById(productId);
                if (currentProduct == null || product == null)
                {
                    return NotFound();
                }

                return Ok(currentProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CartItemDto>> PostItem([FromBody] CartItemToAddDto itemToAdd)
        {
            try
            {
                var newCartItem = await shopingCartService.AddProduct(itemToAdd);

                if (newCartItem == null)
                {
                    return NoContent();
                }

                var product = await productService.GetProductById(newCartItem.ProductId);
                if (product == null)
                {
                    throw new Exception($"Item does not exist in database.(product id: {itemToAdd.ProductId})");
                }
                return CreatedAtAction(nameof(GetCartProductById), new {id=newCartItem.Id}, newCartItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
