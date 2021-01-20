using AS.Data;
using AS.Web.Models;
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
        private readonly ILogger<HomeController> _logger;
        private readonly ASDbContext _context;


        public HomeController(ASDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                List<ProductHomeViewModel> models = await this._context.Products
                    .Select(product => new ProductHomeViewModel
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Description = product.Description
                    })
                    .ToListAsync();

                return this.View("Home", models);
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
