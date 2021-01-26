using AS.Data;
using AS.Data.Models;
using AS.Services.Models;
using AutoMapperTestConfiguration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AS.Services
{
    public class ProductsService : IProductsService
    {
        private readonly ASDbContext db;
        public ProductsService(ASDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> CreateProduct(ProductServiceModel productServiceModel, string stringFileName)
        {
            productServiceModel.ImageUrl = stringFileName;

            Product product = productServiceModel.To<Product>();
            Category category = await this.db.Categories.SingleOrDefaultAsync(category => category.Name == productServiceModel.CategoryServiceModel.Name);

            product.Category = category;

            product.Id = Guid.NewGuid().ToString();

            bool result = await this.db.AddAsync(product) != null;

            await this.db.SaveChangesAsync();

            return result;
        }
    }
}
