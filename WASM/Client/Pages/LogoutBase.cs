using Microsoft.AspNetCore.Components;
using WASM.Client.Services.Contracts;

namespace WASM.Client.Pages
{
    public class LogoutBase : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IAuthService AuthService { get; set; }

        protected override async void OnInitialized()
        {
            await AuthService.Logout();
            NavigationManager.NavigateTo("/");
        }
    }
}
