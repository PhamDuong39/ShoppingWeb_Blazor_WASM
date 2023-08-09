using Microsoft.AspNetCore.Components;
using WASM.Client.Services;
using WASM.Shared.Models;
using WASM.Shared.ViewModels;

namespace WASM.Client.Pages
{
    public class AdminCreateProductBase : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public ProductService productService { get; set; }

        [Inject]
        public CategoryService categoryService { get; set; }

        public ProductViewModel productViewModel { get; set; } = new ProductViewModel();

        public IEnumerable<Category> Categories { get; set; }

        protected override async void OnInitialized()
        {
            Categories = await this.categoryService.GetCategories();
        }
        protected async Task AddProduct_Click()
        {
            var result = await this.productService.CreateProduct(productViewModel);
            if(result !=null)
            {
                NavigationManager.NavigateTo("/ManageProduct");
            }
        }
    }
}
