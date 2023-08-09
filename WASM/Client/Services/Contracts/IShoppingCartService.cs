using WASM.Shared.ViewModels;

namespace WASM.Client.Services.Contracts
{
    public interface IShoppingCartService
    {
        Task<List<CartViewModel>> GetCartItems(string userId);
        Task<CartViewModel> AddToCart(CartViewModel cartViewModel);
        Task<CartViewModel> DeleteItem(int Id);
        Task<CartViewModel> UpdateQty(CartViewModel cartViewModel);

        event Action<int> OnShoppingCartChanged;
        void RaiseEventOnShoppingCartChanged(int totalQty);
    }
}
