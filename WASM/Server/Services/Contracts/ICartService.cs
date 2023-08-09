using WASM.Shared.Models;
using WASM.Shared.ViewModels;

namespace WASM.Server.Services.Contracts
{
    public interface ICartService
    {
        Task<CartItem> AddItemToCart(CartViewModel cartViewModel);
        Task<CartItem> UpdateItemQty(int Id, CartViewModel cartViewModel);
        Task<CartItem> DeleteItemById(int Id);
        Task<CartItem> GetItem(int Id);
        Task<IEnumerable<CartItem>> GetItems(string userId);
    }
}
