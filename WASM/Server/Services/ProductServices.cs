using Microsoft.EntityFrameworkCore;
using WASM.Server.Data;
using WASM.Server.Services.Contracts;
using WASM.Shared.Models;

namespace WASM.Server.Services
{
    public class ProductServices : IProductServices
    {
        private readonly AppDbContext appDbContext;

        public ProductServices(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Product> AddProduct(Product product)
        {
            var result = await this.appDbContext.Products.AddAsync(product);
            await this.appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Product> DeleteProduct(int id)
        {
            var product = await this.appDbContext.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            if (product != null)
            {
                this.appDbContext.Products.Remove(product);
                await this.appDbContext.SaveChangesAsync();
                return product;
            }
            return null;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await this.appDbContext.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await this.appDbContext.Categories.FirstOrDefaultAsync(p => p.CategoryId == id);
        }

        public async Task<Product> GetProductById(int id)
        {
            return await this.appDbContext.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await this.appDbContext.Products.ToListAsync();
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var oldProduct = await this.appDbContext.Products.FirstOrDefaultAsync(x => x.ProductId == product.ProductId);
            if (oldProduct != null)
            {
                oldProduct.ProductId = product.ProductId;
                oldProduct.CategoryId = product.CategoryId;
                oldProduct.ProductName = product.ProductName;
                oldProduct.ImageURL = product.ImageURL;
                oldProduct.Qty = product.Qty;
                oldProduct.Price = product.Price;
                oldProduct.Description = product.Description;

                this.appDbContext.Products.Update(oldProduct);
                await this.appDbContext.SaveChangesAsync();
                return oldProduct;
            }
            return null;
        }
    }
}
