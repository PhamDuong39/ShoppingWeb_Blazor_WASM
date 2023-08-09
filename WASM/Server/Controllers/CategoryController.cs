using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WASM.Client.Services.Contracts;
using WASM.Server.Services;
using WASM.Server.Services.Contracts;
using WASM.Shared.Models;
using WASM.Shared.ViewModels;

namespace WASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IProductServices productServices;

        public CategoryController(IProductServices productServices)
        {
            this.productServices = productServices;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            try
            {
                var categories = await productServices.GetCategories();
                if(categories.Any())
                {
                    return Ok(categories);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                throw;
            }   
        }
    }
}
