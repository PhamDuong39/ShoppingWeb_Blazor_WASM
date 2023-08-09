using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WASM.Client.Services.Contracts;
using WASM.Shared.ViewModels;

namespace WASM.Client.Pages
{
    public class LoginBase : ComponentBase
    {
        [Inject]
        public IAuthService AuthService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public LoginViewModel loginViewModel { get; set; } = new LoginViewModel();

        public string ErrorMessage { get; set; } = "";

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        protected async Task SubmitFormLogin_Click()
        {
            var result = await AuthService.Login(loginViewModel);
            if (result.IsSuccess)
            {
                Console.WriteLine(result.Message);
                
                NavigationManager.NavigateTo("/");
            }
            else
            {
                Console.WriteLine(result.Message);
                ErrorMessage = result.Message;
            }
        }
    }
}
