using WASM.Shared.Models;

namespace WASM.Server.Services.Contracts
{
    public interface IProductServices
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<IEnumerable<Category>> GetCategories();
        Task<Product> GetProductById(int id);
        Task<Category> GetCategoryById(int id);

        // CRUD
        Task<Product> AddProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task<Product> DeleteProduct(int id);
    }
}
