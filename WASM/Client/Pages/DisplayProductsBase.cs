using Microsoft.AspNetCore.Components;
using WASM.Shared.ViewModels;

namespace WASM.Client.Pages
{
    public class DisplayProductsBase : ComponentBase
    {
        [Parameter]
        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
