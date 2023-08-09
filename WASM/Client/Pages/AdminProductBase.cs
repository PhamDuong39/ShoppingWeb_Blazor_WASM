using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using WASM.Client.Services.Contracts;
using WASM.Shared.ViewModels;

namespace WASM.Client.Pages
{
    public class AdminProductBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }

        
        protected override async void OnInitialized()
        {
            Products = await this.ProductService.GetProducts();
        }

    }
}
