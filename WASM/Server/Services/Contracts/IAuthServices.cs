using WASM.Shared.Models;
using WASM.Shared.ViewModels;

namespace WASM.Server.Services.Contracts
{
    public interface IAuthServices
    {
        Task<Response> Login(LoginViewModel loginViewModel);
        Task<Response> Register (RegisterViewModel registerViewModel, string role);  
    }
}
