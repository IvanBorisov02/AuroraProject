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
        private const string noDescriptionMessage = "No Description";

        private readonly ASDbContext db;
        public ProductsService(ASDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> CreateProduct(ProductServiceModel productServiceModel, string stringFileName)
        {
            productServiceModel.ImageUrl = stringFileName;

            if (productServiceModel.Description == null)
            {
                productServiceModel.Description = noDescriptionMessage;
            }


            Product product = productServiceModel.To<Product>();
            Category category = await this.db.Categories.SingleOrDefaultAsync(category => category.Name == productServiceModel.CategoryServiceModel.Name);

            product.Category = category;

            product.Id = Guid.NewGuid().ToString();

            bool result = await this.db.AddAsync(product) != null;

            await this.db.SaveChangesAsync();

            return result;
        }

        public async Task<bool> EditProduct(ProductServiceModel productServiceModel, string stringFileName, string id)
        {
            productServiceModel.ImageUrl = stringFileName;

            Product product = productServiceModel.To<Product>();

            Category category = await this.db.Categories.SingleOrDefaultAsync(category => category.Name == productServiceModel.CategoryServiceModel.Name);

            product.Category = category;
            product.Id = id;

            bool result = this.db.Update(product) != null;
            await this.db.SaveChangesAsync();

            return result;

        }

        public async Task<bool> DeleteProduct(string id)
        {
            Product product = await this.db.Products.Include(product => product.Category).SingleOrDefaultAsync(product => product.Id == id);

            bool result = this.db.Remove(product) != null;
            await this.db.SaveChangesAsync();

            return result;
        }
    }
}
