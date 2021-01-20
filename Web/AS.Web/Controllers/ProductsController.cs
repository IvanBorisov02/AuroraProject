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
using AS.Services;
using AS.Services.Models;
using AutoMapperTestConfiguration;

namespace AS.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ASDbContext _context;
        private readonly IWebHostEnvironment WebHostEnvironment;
        private readonly IProductsService productsService;


        public ProductsController(ASDbContext context, IWebHostEnvironment webHostEnvironment, IProductsService productsService)
        {
            _context = context;
            WebHostEnvironment = webHostEnvironment;
            this.productsService = productsService;
        }


        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public IActionResult Create()
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

            ProductServiceModel serviceModel = productCreateViewModel.To<ProductServiceModel>();

            string stringFileName = UploadFile(productCreateViewModel);

            await productsService.CreateProduct(serviceModel, stringFileName);

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

            ProductEditViewModel productEditViewModel = new ProductEditViewModel
            {
                Id = product.Id,
                Price = product.Price,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category.Name,
                ImageUrl = product.ImageUrl
            };

            return View(productEditViewModel);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, ProductEditViewModel productEditViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            string fileName = UploadFile(productEditViewModel);

            //productServices.EditProduct(id, productEditViewModel, fileName);

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
        public async Task<IActionResult> DeleteConfirm(string id)
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

        private string UploadFile(ProductEditViewModel productEditViewModel)
        {
            string fileName = null;
            if (productEditViewModel.Image != null)
            {
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "Images");
                fileName = Guid.NewGuid().ToString() + "-" + productEditViewModel.Image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    productEditViewModel.Image.CopyTo(fileStream);
                }
            }
            return fileName;
        }
    }
}
