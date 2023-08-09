using System.Net;
using System.Net.Http.Json;
using WASM.Client.Services.Contracts;
using WASM.Shared.Models;
using WASM.Shared.ViewModels;

namespace WASM.Client.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient httpClient;

        public ProductService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ProductViewModel> CreateProduct(ProductViewModel productViewModel)
        {
            try
            {
                var response = await this.httpClient.PostAsJsonAsync<ProductViewModel>("api/Product/CreateProduct", productViewModel);
                if(response.IsSuccessStatusCode)
                {
                    if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(ProductViewModel);
                    }
                    return await response.Content.ReadFromJsonAsync<ProductViewModel>();  
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Product> DeleteProduct(int Id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/Product/DeleteProduct/{Id}");
                if (response.IsSuccessStatusCode)
                {
                    // Xóa thành công, trả về đối tượng Product đã bị xóa
                    var deletedProduct = await response.Content.ReadFromJsonAsync<Product>();
                    return deletedProduct;
                }

                throw new Exception($"Error deleting item. Status code: {response.StatusCode}");
            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task<ProductViewModel> GetProduct(int Id)
        {
            try
            {
                var product = await httpClient.GetFromJsonAsync<ProductViewModel>($"api/Product/{Id}");
                return product;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProductViewModel>> GetProducts()
        {
            try
            {
                var products = await this.httpClient.GetFromJsonAsync<IEnumerable<ProductViewModel>>("api/Product");
                return products;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductViewModel> UpdateProduct(ProductViewModel productViewModel)
        {
            try
            {
                var response = await this.httpClient.PutAsJsonAsync<ProductViewModel>("api/product/UpdateProduct", productViewModel);
                return await response.Content.ReadFromJsonAsync<ProductViewModel>();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
