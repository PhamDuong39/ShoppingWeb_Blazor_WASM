using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WASM.Server.Services.Contracts;
using WASM.Shared.ViewModels;

namespace WASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices authServices;

        public AuthController(IAuthServices authServices)
        {
            this.authServices = authServices;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            var result = await this.authServices.Login(loginViewModel);
            if(result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterViewModel registerVM, string role)
        {
            var result = await this.authServices.Register(registerVM, role);
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
