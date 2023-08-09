using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WASM.Client.Services.Contracts;
using WASM.Server.Services.Contracts;
using WASM.Shared.Models;
using WASM.Shared.ViewModels;

namespace WASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CartController : ControllerBase
    {
        private readonly IProductServices productService;
        private readonly ICartService cartService;

        public CartController(IProductServices productService, ICartService cartService)
        {
            this.productService = productService;
            this.cartService = cartService;
        }

        [HttpGet("{userId}/GetItems")]
        public async Task<ActionResult<IEnumerable<CartViewModel>>> GetItems(string userId)
        {
            try
            {
              
                // Lay du lieu gio hang voi tung nguoi dung
                var cartItems = await this.cartService.GetItems(userId);
                if(cartItems == null)
                {
                    return NotFound();
                }
                var products = await this.productService.GetProducts();
                if(products == null)
                {
                    throw new Exception("No products in the Database");
                }
                var cartItemVMs = (from cartItem in cartItems
                                   join product in products on cartItem.ProductId equals product.ProductId
                                   select new CartViewModel
                                   {
                                       Id = cartItem.CartItemId,
                                       CartId = cartItem.ProductId,
                                       ProductId = cartItem.ProductId,
                                       ProductName = product.ProductName,
                                       ProductDescription = product.Description,
                                       ProductImageURL = product.ImageURL,
                                       Price = product.Price,
                                       Qty = cartItem.Qty,
                                       TotalPrice = product.Price * cartItem.Qty
                                   }).ToList();
                return Ok(cartItemVMs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<CartViewModel>> GetItem(int Id)
        {
            try
            {
                var cartItem = await this.cartService.GetItem(Id);
                if(cartItem == null) { return NotFound(); }
                var product = await this.productService.GetProductById(cartItem.ProductId);
                if(product == null) { return NotFound(); };
                return Ok(new CartViewModel
                {
                    Id = cartItem.CartItemId,
                    CartId = cartItem.ProductId,
                    ProductId = cartItem.ProductId,
                    ProductName = product.ProductName,
                    ProductDescription = product.Description,
                    ProductImageURL = product.ImageURL,
                    Price = product.Price,
                    Qty = product.Qty,
                    TotalPrice = product.Price * cartItem.Qty
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
       
        public async Task<ActionResult<CartViewModel>> PostItem(CartViewModel cartViewModel)
        {
            try
            {
                var newCartItem = await this.cartService.AddItemToCart(cartViewModel);
                if(newCartItem == null) { return NoContent(); }
                var product = await this.productService.GetProductById(newCartItem.ProductId);
                if(product == null)
                {
                    return BadRequest();
                }
                var newCartItemVM = new CartViewModel
                {
                    Id = newCartItem.CartItemId,
                    CartId = newCartItem.ProductId,
                    ProductId = newCartItem.ProductId,
                    ProductName = product.ProductName,
                    ProductDescription = product.Description,
                    ProductImageURL = product.ImageURL,
                    Price = product.Price,
                    Qty = product.Qty,
                    TotalPrice = product.Price * newCartItem.Qty
                };
                return CreatedAtAction(nameof(GetItem), new {id = newCartItemVM.Id}, newCartItemVM);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpDelete("{Id:int}")]
        public async Task<ActionResult<CartViewModel>> DeleteItem(int Id)
        {
            try
            {
                var cartItem = await this.cartService.DeleteItemById(Id);
                if(cartItem == null)
                {
                    return NotFound();
                }
                var product = await this.productService.GetProductById(cartItem.ProductId);
                if(product == null)
                {
                    return NotFound();
                }

                return Ok(new CartViewModel
                {
                    Id = cartItem.CartItemId,
                    CartId = cartItem.ProductId,
                    ProductId = cartItem.ProductId,
                    ProductName = product.ProductName,
                    ProductDescription = product.Description,
                    ProductImageURL = product.ImageURL,
                    Price = product.Price,
                    Qty = product.Qty,
                    TotalPrice = product.Price * cartItem.Qty
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{Id:int}")]
        public async Task<ActionResult<CartViewModel>> UpdateQty(int Id, CartViewModel cartViewModel)
        {
            try
            {
                var cartItem = await this.cartService.UpdateItemQty(Id, cartViewModel);
                if(cartItem == null)
                {
                    return NotFound();
                }

                var product = await this.productService.GetProductById(cartItem.ProductId);
                return Ok(new CartViewModel
                {
                    Id = cartItem.CartItemId,
                    CartId = cartItem.ProductId,
                    ProductId = cartItem.ProductId,
                    ProductName = product.ProductName,
                    ProductDescription = product.Description,
                    ProductImageURL = product.ImageURL,
                    Price = product.Price,
                    Qty = product.Qty,
                    TotalPrice = product.Price * cartItem.Qty
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
