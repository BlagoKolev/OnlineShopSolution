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
        private readonly IShoppingCartService shoppingCartService;

        public ShoppingCartController(IProductService productService,
            IShoppingCartService shoppingCartService)
        {
            this.productService = productService;
            this.shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        [Route("{userId}/GetProducts")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetProducts(int userId)
        {
            try
            {
                var productsInCart = await shoppingCartService.GetAllProductsInCart(userId);
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
                var currentProduct = await shoppingCartService.GetProductInCart(productId, cartId);
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
                var newCartItem = await shoppingCartService.AddProduct(itemToAdd);

                if (newCartItem == null)
                {
                    return NoContent();
                }

                var product = await productService.GetProductById(newCartItem.ProductId);
                if (product == null)
                {
                    throw new Exception($"Item does not exist in database.(product id: {itemToAdd.ProductId})");
                }
                return CreatedAtAction(nameof(GetCartProductById), new { id = newCartItem.Id }, newCartItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CartItemDto>> DeleteItem(int id)
        {
            try
            {
                var itemToDelete = await shoppingCartService.RemoveFromShopingCart(id);

                if (itemToDelete == null)
                {
                    return NotFound();
                }

                var product = productService.GetProductById(itemToDelete.ProductId);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(itemToDelete);

            }
            catch (Exception ex)
            {
                 return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
