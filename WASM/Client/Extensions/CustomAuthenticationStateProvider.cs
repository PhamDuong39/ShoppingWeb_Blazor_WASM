using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace WASM.Client.Extensions
{
    // Quản lý trạng thái xác thực của User dựa trên thông tin Token được lưu trữ trong LocalStorage
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorageService;

        public CustomAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            this.httpClient = httpClient;
            this.localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Lấy thông tin xác thực từ LocalStorage
            var savedToken = await localStorageService.GetItemAsync<string>("authToken");
            
            if(string.IsNullOrWhiteSpace(savedToken))
            {
                // Nếu không tìm thấy thông tin xác thực => Trả về 1 AuthenticationState với 1 ClaimPrincical trông (Ko có thông tin User)
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // Ngược lại, Nếu có thông tin xác thực thì sẽ Update DefaultRequestHeaders của HttpClient bằng cách thêm Title - Bearer = Token đã lấy
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(savedToken), "jwt")));
        }

        // Đánh dấu User (Authenticated)
        public void MarkUserAsAuthenticatedAsync(string userName)
        {
            var authState = GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }


        // Laasy các thông tin từ JWT (trong payload của Token) để khai báo các claims
        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            // Tách phần thân của Payload Token
            var payload = jwt.Split('.')[1];
            // Giải mã Base64 sử dụng ParseBase64UrlWithoutPadding thay vì ParseBase64WithoutPadding
            var jsonBytes = ParseBase64WithoutPadding(payload);
            // Phần thân sau khi giải mã sẽ được chuyển thành 1 object Dictionary
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);


            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);
            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;

        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }


    }
}
