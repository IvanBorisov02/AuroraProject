using AS.Data;
using AS.Web.Models;
using AS.Web.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AS.Web.Controllers
{
    public class HomeController : Controller
    {
        private const int PageSize = 5;
        private readonly ILogger<HomeController> _logger;
        private readonly ASDbContext _context;


        public HomeController(ASDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(ProductHomeViewModel model)
        {
            model.Pager ??= new PagerViewModel();
            model.Pager.CurrentPage = model.Pager.CurrentPage <= 0 ? 1 : model.Pager.CurrentPage;

            if (this.User.Identity.IsAuthenticated)
            {
                List<ProductViewModel> models = await _context.Products.Skip((model.Pager.CurrentPage - 1) * PageSize).Take(PageSize).Select(product => new ProductViewModel()
                {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Description = product.Description,
                        ImageUrl = product.ImageUrl
                    })
                    .ToListAsync();

                model.Products = models;
                model.Pager.PagesCount = (int)Math.Ceiling(await _context.Products.CountAsync() / (double)PageSize);

                return this.View("Home", model);
            }

            return View();
        }

        public async Task<IActionResult> Categorize(string categoryName, string genderTypeName, ProductCategorizedHomeViewModel model)
        {
            model.Pager ??= new PagerViewModel();
            model.Pager.CurrentPage = model.Pager.CurrentPage <= 0 ? 1 : model.Pager.CurrentPage;

            List<ProductCategorizedViewModel> models = new List<ProductCategorizedViewModel>();

            if (this.User.Identity.IsAuthenticated)
            {
                 models = await this._context.Products
                    .Where(product => (product.Category.Name == categoryName) && product.GenderType.Name == genderTypeName)
                    .Select(product => new ProductCategorizedViewModel
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Description = product.Description,
                        CategoryName = categoryName,
                        ImageUrl = product.ImageUrl
                    })
                    .ToListAsync();

               

                if (models.Count == 0)
                {
                    ProductEmptyViewModel error = new ProductEmptyViewModel { CategoryName = categoryName };

                    return this.View("Empty", error);
                }

                model.Products = models;
                model.Pager.PagesCount = (int)Math.Ceiling( models.Count / (double)PageSize);
                return this.View("Categorized", model);
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
