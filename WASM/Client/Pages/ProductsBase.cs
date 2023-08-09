using Microsoft.AspNetCore.Components;
using WASM.Client.Services.Contracts;
using WASM.Shared.ViewModels;

namespace WASM.Client.Pages
{
    public class ProductsBase : ComponentBase
    {
        [Inject]
        public IProductService productService { get; set; }
        [Inject]
        public IShoppingCartService shoppingCartService { get; set; }
        public IEnumerable<ProductViewModel>  Products { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Products = await productService.GetProducts();
            var shoppingCartItems = await shoppingCartService.GetCartItems(FakeUserLogin.userId);
            var totalQty = shoppingCartItems.Sum(x => x.Qty);

            // Gọi Method RaiseEventOnShoppingCartChanged => Sự kiện OnShoppingCartChanged Sẽ được kích hoạt
            this.shoppingCartService.RaiseEventOnShoppingCartChanged(totalQty);
        }
    }
}
