using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WASM.Server.Services;
using WASM.Server.Services.Contracts;
using WASM.Shared.Models;
using WASM.Shared.ViewModels;

namespace WASM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices productServices;

        public ProductController(IProductServices productServices)
        {
            this.productServices = productServices;
        }

        

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetProducts()
        {
            try
            {
                var products = await productServices.GetProducts();
                var categories = await productServices.GetCategories();


                var productVMs = (from a in products
                                  join b in categories on a.CategoryId equals b.CategoryId
                                  select new ProductViewModel
                                  {
                                      ProductId = a.ProductId,
                                      CategoryId = b.CategoryId,
                                      Name = a.ProductName,
                                      Price = a.Price,
                                      CategoryName = b.CategoryName,
                                      Description = a.Description,
                                      ImageURL = a.ImageURL,
                                      Qty = a.Qty,
                                  });
                if (products == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(productVMs);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Can not retrieving data from the DB");
            }
        }

        [HttpGet("{Id:int}")]
     
        public async Task<ActionResult<ProductViewModel>> GetProductById(int Id)
        {
            try
            {
                var product = await this.productServices.GetProductById(Id);
                if(product == null)
                {
                    return BadRequest();
                }
                else
                {
                    var category = await this.productServices.GetCategoryById(product.CategoryId);
                    return new ProductViewModel
                    {
                        ProductId = product.ProductId,
                        CategoryId = category.CategoryId,
                        Name = product.ProductName,
                        Price = product.Price,
                        CategoryName = category.CategoryName,
                        Description = product.Description,
                        ImageURL = product.ImageURL,
                        Qty = product.Qty,
                    };
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Can not retrieving data from the DB");
            }
        }

        // CRUD
        [HttpPost("CreateProduct")]
        public async Task<ActionResult<ProductViewModel>> AddProduct(ProductViewModel productViewModel)
        {
            try
            {
                if (productViewModel == null)
                {
                    return BadRequest();
                }

                var product = new Product
                {
                    ProductName = productViewModel.Name,
                    Description = productViewModel.Description,
                    ImageURL = productViewModel.ImageURL,
                    Price = productViewModel.Price,
                    Qty = productViewModel.Qty,
                    CategoryId = productViewModel.CategoryId
                };

                var createdProduct = await this.productServices.AddProduct(product);
                var category = await this.productServices.GetCategoryById(product.CategoryId);
                // Tạo ProductViewModel từ Product
                var createdProductViewModel = new ProductViewModel
                {
                    ProductId = createdProduct.ProductId,
                    Name = createdProduct.ProductName,
                    Description = createdProduct.Description,
                    ImageURL = createdProduct.ImageURL,
                    Price = createdProduct.Price,
                    Qty = createdProduct.Qty,
                    CategoryId = createdProduct.CategoryId,
                    CategoryName = category.CategoryName 
                };

                return CreatedAtAction(nameof(AddProduct), new { id = createdProductViewModel.ProductId }, createdProductViewModel);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Không thể lấy dữ liệu từ DB");
            }
        }

        [HttpPut("UpdateProduct")]
        public async Task<ActionResult<ProductViewModel>> UpdateProduct(ProductViewModel productViewModel)
        {
            try
            {
                var productToUpdate = await this.productServices.GetProductById(productViewModel.ProductId);
                if (productToUpdate == null)
                {
                    return NotFound();
                }

                productToUpdate.ProductName = productViewModel.Name;
                productToUpdate.Description = productViewModel.Description;
                productToUpdate.ImageURL = productViewModel.ImageURL;
                productToUpdate.Price = productViewModel.Price;
                productToUpdate.Qty = productViewModel.Qty;
                productToUpdate.CategoryId = productViewModel.CategoryId;

                var updatedProduct = await this.productServices.UpdateProduct(productToUpdate);

                var category = await this.productServices.GetCategoryById(updatedProduct.CategoryId);
                // Tạo ProductViewModel từ Product
                var updatedProductViewModel = new ProductViewModel
                {
                    ProductId = updatedProduct.ProductId,
                    Name = updatedProduct.ProductName,
                    Description = updatedProduct.Description,
                    ImageURL = updatedProduct.ImageURL,
                    Price = updatedProduct.Price,
                    Qty = updatedProduct.Qty,
                    CategoryId = updatedProduct.CategoryId,
                    CategoryName = category.CategoryName // Giả sử Category có thuộc tính Name
                };

                return updatedProductViewModel;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Không thể lấy dữ liệu từ DB");
            }
        }



        [HttpDelete("DeleteProduct/{Id:int}")]
        public async Task<ActionResult<Product>> DeleteProduct(int Id)
        {
            try
            {
                var productToDelete = await this.productServices.GetProductById(Id);
                if(productToDelete == null)
                {
                    return NotFound();
                }
                return await this.productServices.DeleteProduct(Id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Không thể lấy dữ liệu từ DB");
            }
        }
    }
}
