using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;
using System.Net.Http.Json;
using WASM.Client.Services.Contracts;
using WASM.Shared.Models;
using WASM.Shared.ViewModels;
using WASM.Client.Extensions;
using System.Net.Http.Headers;

namespace WASM.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorageService;
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public AuthService(HttpClient httpClient, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider)
        {
            this.httpClient = httpClient;
            this.localStorageService = localStorageService;
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<Response> Login(LoginViewModel loginViewModel)
        {
            // Post loginViewModel Lên api
            var result = await this.httpClient.PostAsJsonAsync("api/Auth/Login",loginViewModel);
            // Lấy dữ liệu được trả về từ API
            var content = await result.Content.ReadAsStringAsync();
            // Đọc Json
            var loginResponse = JsonSerializer.Deserialize<Response>(content, new JsonSerializerOptions()
            {
                // Dữ liệu nhaỵ cảm
                PropertyNameCaseInsensitive = true
            });
            //if(result.IsSuccessStatusCode)
            //{
            //    await this.localStorageService.SetItemAsync("authToken", loginResponse.Token);
            //    // Ép kiểu cho authenticationStateProvider => Đánh dấu là username đã đăng nhập
            //    ((CustomAuthenticationStateProvider)this.authenticationStateProvider).MarkUserAsAuthenticatedAsync(loginViewModel.Username);
            //    this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse.Token);
            //    return loginResponse;
            //}
            //return loginResponse;

            if (!result.IsSuccessStatusCode)
            {
                return loginResponse;
            }
            await this.localStorageService.SetItemAsync("authToken", loginResponse.Token);
            ((CustomAuthenticationStateProvider)this.authenticationStateProvider).MarkUserAsAuthenticatedAsync(loginViewModel.Username); 
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse.Token);
            return loginResponse;
        }

        public async Task Logout()
        {
            // Xóa cái authToken trong localStorage đi
            await this.localStorageService.RemoveItemAsync("authToken");
            ((CustomAuthenticationStateProvider)this.authenticationStateProvider).MarkUserAsLoggedOut();
            this.httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
