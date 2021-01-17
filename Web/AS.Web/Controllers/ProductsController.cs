using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AS.Data;
using AS.Data.Models;
using Microsoft.AspNetCore.Authorization;
using AS.Web.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace AS.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ASDbContext _context;
        private readonly IWebHostEnvironment WebHostEnvironment;

        public ProductsController(ASDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            WebHostEnvironment = webHostEnvironment;
        }


        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ProductCreateViewModel productCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            string stringFileName = UploadFile(productCreateViewModel);

            Product product = new Product
            {
                Id = Guid.NewGuid().ToString(),
                Price = productCreateViewModel.Price,
                Name = productCreateViewModel.Name,
                Description = productCreateViewModel.Description,
                Category = await this._context.Categories.SingleOrDefaultAsync(x => x.Name == productCreateViewModel.Category),
                ImageUrl = stringFileName
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
                Category = product.Category.Name,
                ImageUrl = product.ImageUrl
            };

            return this.View(productDetailsViewModel);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            Product product = await this._context.Products.Include(product => product.Category).SingleOrDefaultAsync(product => product.Id == id);

            ProductEditCommonModel productEditCommonModel = new ProductEditCommonModel
            {
                Id = product.Id,
                Price = product.Price,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category.Name,
                ImageUrl = product.ImageUrl
            };

            return View(productEditCommonModel);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, ProductEditBindingModel productEditBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Product product = await this._context.Products.Include(product => product.Category).SingleOrDefaultAsync(product => product.Id == id);

            product.Price = productEditBindingModel.Price;
            product.Name = productEditBindingModel.Name;
            product.Description = productEditBindingModel.Description;
            product.Category = await this._context.Categories.SingleOrDefaultAsync(x => x.Name == productEditBindingModel.Category);

            string fileName = UploadFile(productEditBindingModel);

            product.ImageUrl = fileName;

            this._context.Update(product);
            await this._context.SaveChangesAsync();

            return Redirect("/");
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            Product product = await this._context.Products.Include(product => product.Category).SingleOrDefaultAsync(product => product.Id == id);

            ProductDeleteViewModel productDeleteViewModel = new ProductDeleteViewModel
            {
                Id = product.Id,
                Price = product.Price,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category.Name,
                ImageUrl = product.ImageUrl
            };

            return View(productDeleteViewModel);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
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

        private string UploadFile(ProductCreateViewModel productCreateViewModel)
        {
            string fileName = null;
            if (productCreateViewModel.Image != null)
            {
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "Images");
                fileName = Guid.NewGuid().ToString() + "-" + productCreateViewModel.Image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    productCreateViewModel.Image.CopyTo(fileStream);
                }
            }
            return fileName;
        }

        private string UploadFile(ProductEditBindingModel productEditBindingModel)
        {
            string fileName = null;
            if (productEditBindingModel.Image != null)
            {
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "Images");
                fileName = Guid.NewGuid().ToString() + "-" + productEditBindingModel.Image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    productEditBindingModel.Image.CopyTo(fileStream);
                }
            }
            return fileName;
        }
    }
}
