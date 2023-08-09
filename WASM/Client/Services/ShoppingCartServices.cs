using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text;
using WASM.Client.Services.Contracts;
using WASM.Shared.ViewModels;
using Newtonsoft.Json;

namespace WASM.Client.Services
{
    public class ShoppingCartServices : IShoppingCartService
    {
        private readonly HttpClient httpClient;

        // Delegate Event Action
        // Define Event Named OnShoppingCartChanged => Active the Trigger whenever the Quantity of the Item in the
        // Cart changed
        public event Action<int> OnShoppingCartChanged;

        public ShoppingCartServices(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

       

        public async Task<CartViewModel> AddToCart(CartViewModel cartViewModel)
        {
            try
            {
                var response = await this.httpClient.PostAsJsonAsync<CartViewModel>("api/Cart", cartViewModel);
                if(response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(CartViewModel);
                    }
                    return await response.Content.ReadFromJsonAsync<CartViewModel>();
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

        public async Task<CartViewModel> DeleteItem(int Id)
        {
            try
            {
                var response = await this.httpClient.DeleteAsync($"api/Cart/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    var cart = await response.Content.ReadFromJsonAsync<CartViewModel>();
                    return cart;
                }
                else
                {
                    throw new Exception($"Error deleting item. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting item: {ex.Message}");
            }
        }

        public async Task<List<CartViewModel>> GetCartItems(string userId)
        {
            try
            {
                var cartItems = await this.httpClient.GetFromJsonAsync<IEnumerable<CartViewModel>>($"api/Cart/{userId}/GetItems");
                return cartItems.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }


        // Use to Active the Event OnShoppingCartChanged by use the Invoke Method
        public void RaiseEventOnShoppingCartChanged(int totalQty)
        {
            if(OnShoppingCartChanged != null)
            {
                // Invoke : được sử dụng để gọi các Method được tham chiếu bởi Delegate
                // Invoke Method : kích hoạt các Method được tham chiếu và chạy chúng
                // Event OnShoppingCartChanged sẽ được gọi ngay sau khi giỏ hảng có sự thay đổi số lượng
                OnShoppingCartChanged.Invoke(totalQty);
            }
        }

        public async Task<CartViewModel> UpdateQty(CartViewModel cartViewModel)
        {
            //https://localhost:7192/api/Cart/10
            try
            {
                var jsonRequest = JsonConvert.SerializeObject(cartViewModel);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await this.httpClient.PutAsync($"api/Cart/{cartViewModel.Id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CartViewModel>();
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
