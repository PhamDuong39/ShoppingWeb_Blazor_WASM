using WASM.Shared.Models;

namespace WASM.Client.Services.Contracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategories();
    }
}
