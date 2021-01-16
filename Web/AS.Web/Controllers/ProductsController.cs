using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AS.Data;
using AS.Data.Models;
using Microsoft.AspNetCore.Authorization;
using AS.Web.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AS.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ASDbContext _context;

        public ProductsController(ASDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ProductCreateViewModel productCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Product product = new Product
            {
                Id = Guid.NewGuid().ToString(),
                Price = productCreateViewModel.Price,
                Name = productCreateViewModel.Name,
                Description = productCreateViewModel.Description,
                Category = await this._context.Categories.SingleOrDefaultAsync(x => x.Name == productCreateViewModel.Category)
            };

            await this._context.AddAsync(product);
            await this._context.SaveChangesAsync();

            return Redirect("/");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            Product product = await this._context.Products.Include(product => product.Category).SingleOrDefaultAsync(product => product.Id == id);

            ProductDetailsViewModel productDetailsViewModel = new ProductDetailsViewModel
            {
                Id = product.Id,
                Price = product.Price,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category.Name
            };

            return this.View(productDetailsViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            Product product = await this._context.Products.Include(product => product.Category).SingleOrDefaultAsync(product => product.Id == id);

            ProductEditCommonModel productEditCommonModel = new ProductEditCommonModel
            {
                Id = product.Id,
                Price = product.Price,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category.Name
            };

            return View(productEditCommonModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, ProductEditCommonModel productEditCommonModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Product product = await this._context.Products.Include(product => product.Category).SingleOrDefaultAsync(product => product.Id == id);

            product.Price = productEditCommonModel.Price;
            product.Name = productEditCommonModel.Name;
            product.Description = productEditCommonModel.Description;
            product.Category = await this._context.Categories.SingleOrDefaultAsync(x => x.Name == productEditCommonModel.Category);

            this._context.Update(product);
            await this._context.SaveChangesAsync();

            return Redirect("/");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            Product product = await this._context.Products.Include(product => product.Category).SingleOrDefaultAsync(product => product.Id == id);

            ProductDeleteViewModel productDeleteViewModel = new ProductDeleteViewModel
            {
                Id = product.Id,
                Price = product.Price,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category.Name
            };

            return View(productDeleteViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(string id, ProductEditCommonModel productEditCommonModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Delete");
            }

            Product product = await this._context.Products.Include(product => product.Category).SingleOrDefaultAsync(product => product.Id == id);

            this._context.Remove(product);
            await this._context.SaveChangesAsync();

            return Redirect("/");
        }
    }
}
