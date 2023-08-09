using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WASM.Server.Data;
using WASM.Server.Services.Contracts;
using WASM.Shared.Models;
using WASM.Shared.ViewModels;

namespace WASM.Server.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly AppDbContext appDbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public AuthServices(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext appDbContext, IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            this.configuration = configuration;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<Response> Login(LoginViewModel loginViewModel)
        {
            // check user Da ton tai hay chua
            var user = await this.userManager.FindByNameAsync(loginViewModel.Username);
            if (user == null)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Wrong password or username"
                };
            }
            else if(!await this.userManager.CheckPasswordAsync(user, loginViewModel.Password))
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Wrong password or username"
                };
            }
            else
            {
                var roles = await this.userManager.GetRolesAsync(user);

                // Tao ra danh sach cac claim de an trong token
                var claims = new List<Claim>()
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email.ToString()),
                    new Claim(ClaimTypes.Role, roles.First())
                };

                // Tao JWT token
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["JWT:SecretKey"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken
                (
                    this.configuration["JWT:Issuer"],
                    this.configuration["JWT:Audience"],
                    claims, 
                    expires: DateTime.Now.AddDays(15), 
                    signingCredentials: signIn
                );
                
                return new Response
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Login successfuly",
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                };
            }
        }

        public async Task<Response> Register(RegisterViewModel registerViewModel,string role)
        {
            // Check xem User Da ton tai hay chua
            if(await this.userManager.FindByEmailAsync(registerViewModel.Email) != null)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Email has already exist"
                };
            }
            else if(await this.userManager.FindByNameAsync(registerViewModel.Username) != null)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Username has already exist"
                };
            }
            else
            {
                // Create IdentityUser 
                ApplicationUser applicationUser = new ApplicationUser()
                {
                    UserName = registerViewModel.Username,
                    Email = registerViewModel.Email,
                    FirstName = registerViewModel.FirstName,
                    LastName = registerViewModel.LastName
                };

                // Check role co ton tai hay khong
                if(await this.roleManager.RoleExistsAsync(role))
                {
                    // Them user vao Database
                    var result = await this.userManager.CreateAsync(applicationUser, registerViewModel.Password);
                    //Check dang ky that bai
                    if (!result.Succeeded)
                    {
                        return new Response
                        {
                            IsSuccess = false,
                            StatusCode = StatusCodes.Status400BadRequest,
                            Message = "Something went wrong when register your account"
                        };
                    }

                    // Add role to user
                    await this.userManager.AddToRoleAsync(applicationUser, role);
                    return new Response
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status201Created,
                        Message = "Account has been created Successfuly"
                    };
                }
                else
                {
                    return new Response
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Role is not exist"
                    };
                }
            }
        }
    }
}
