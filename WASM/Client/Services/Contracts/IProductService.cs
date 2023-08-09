using WASM.Shared.Models;
using WASM.Shared.ViewModels;

namespace WASM.Client.Services.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> GetProducts();
        Task<ProductViewModel> GetProduct(int Id);


        // CRUD
        Task<ProductViewModel> CreateProduct(ProductViewModel productViewModel);
        Task<ProductViewModel> UpdateProduct(ProductViewModel productViewModel);
        Task<Product> DeleteProduct(int Id);
    }
}
