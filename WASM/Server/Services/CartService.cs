using Microsoft.EntityFrameworkCore;
using WASM.Server.Data;
using WASM.Server.Services.Contracts;
using WASM.Shared.Models;
using WASM.Shared.ViewModels;

namespace WASM.Server.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext appDbContext;

        public CartService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        private async Task<bool> CartItemExists(int cartId, int productId)
        {
            return await this.appDbContext.CartItems.AnyAsync(p => p.CartId == cartId && p.ProductId == productId);
        }

        public async Task<CartItem> AddItemToCart(CartViewModel cartViewModel)
        {
            // Check xem da ton tai Cart voi Product nay hay chua
            if(await CartItemExists(cartViewModel.CartId, cartViewModel.ProductId) == false)
            {
                var products = await this.appDbContext.Products.ToListAsync();
                var item = (from product in products
                            where product.ProductId == cartViewModel.ProductId
                            select new CartItem
                            {
                                CartId = cartViewModel.CartId,
                                ProductId = product.ProductId,
                                Qty = cartViewModel.Qty
                            }).FirstOrDefault();
                if (item != null)
                {
                    var result = await this.appDbContext.CartItems.AddAsync(item);
                    await this.appDbContext.SaveChangesAsync();
                    return result.Entity;
                }
            }
           
            return null;
        }

        public async Task<CartItem> DeleteItemById(int Id)
        {
            var item = await this.appDbContext.CartItems.FindAsync(Id);
            if(item != null)
            {
                this.appDbContext.CartItems.Remove(item);
                await this.appDbContext.SaveChangesAsync();
            }
            return item;
        }

        public async Task<CartItem> GetItem(int Id)
        {
            var carts = await this.appDbContext.Carts.ToListAsync();
            var cartItems = await this.appDbContext.CartItems.ToListAsync();
            return (from cart in carts
                    join cartItem in cartItems on cart.CartId equals cartItem.CartId
                    where cartItem.CartItemId == Id
                    select new CartItem
                    {
                        CartItemId = cartItem.CartItemId,
                        ProductId = cartItem.ProductId,
                        Qty = cartItem.Qty,
                        CartId = cartItem.CartId
                    }).FirstOrDefault();
        }

        public async Task<IEnumerable<CartItem>> GetItems(string userId)
        {
            var carts = await this.appDbContext.Carts.ToListAsync();
            var cartItems = await this.appDbContext.CartItems.ToListAsync();
            return (from cart in carts
                          join cartItem in cartItems on cart.CartId equals cartItem.CartId
                          where cart.UserId == userId
                          select new CartItem
                          {
                              CartItemId = cartItem.CartItemId,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartId = cartItem.CartId
                          }).ToList();
        }

        public async Task<CartItem> UpdateItemQty(int Id, CartViewModel cartViewModel)
        {
            var item = await this.appDbContext.CartItems.FindAsync(Id);
            if(item !=null)
            {
                item.Qty = cartViewModel.Qty;
                await this.appDbContext.SaveChangesAsync();
                return item;
            }
            return null;
        }
    }
}
