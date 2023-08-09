using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata;
using WASM.Client.Services.Contracts;
using WASM.Shared.Models;
using WASM.Shared.ViewModels;

namespace WASM.Client.Pages
{
    public class ProductDetailsBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IProductService productService { get; set; }
        [Inject]
        public IShoppingCartService shoppingCartService { get; set; }
        public ProductViewModel Product { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }
     
        // Day du lieu san pham
        protected override async Task OnInitializedAsync()
        {
            Product = await productService.GetProduct(Id);
        }

        // Add Skien cho button Add to cart
        protected async Task AddToCart_Click( CartViewModel cartViewModel)
        {
            try
            {
                var cartVM = await shoppingCartService.AddToCart(cartViewModel);
                NavigationManager.NavigateTo("/ShoppingCart");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
