using System.Net.Http.Json;
using WASM.Client.Services.Contracts;
using WASM.Shared.Models;

namespace WASM.Client.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient httpClient;

        public CategoryService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            var categories = await this.httpClient.GetFromJsonAsync<IEnumerable<Category>>("api/category");
            return categories;
        }
    }
}
