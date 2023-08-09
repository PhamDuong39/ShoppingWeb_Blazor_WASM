using WASM.Shared.Models;
using WASM.Shared.ViewModels;

namespace WASM.Client.Services.Contracts
{
    public interface IAuthService
    {
        Task<Response> Login(LoginViewModel loginViewModel);
        Task Logout();
    }
}
