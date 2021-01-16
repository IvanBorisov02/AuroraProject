using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AS.Data;
using AS.Data.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using AS.Web.Models;

namespace AS.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ASDbContext _context;

        public OrdersController(ASDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Order(string id)
        {
            Product product = await this._context.Products
                .Include(product => product.Category)
                .SingleOrDefaultAsync(product => product.Id == id);

            string claim = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            ASUser aSUser = await this._context.Users
                .SingleOrDefaultAsync(user => user.Id == claim);

            Order order = new Order
            {
                Id = Guid.NewGuid().ToString(),
                OrderedOn = DateTime.Now,
                Product = product,
                Orderer = aSUser
            };

            await this._context.AddAsync(order);
            await this._context.SaveChangesAsync();

            return Redirect($"/Products/Details/{id}");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllAsync()
        {
            List<OrderAllViewModel> models = await this._context.Orders
                .Include(order => order.Orderer)
                .Include(order => order.Product)
                .Select(order => new OrderAllViewModel
                {
                    Id = order.Id,
                    Orderer = order.Orderer.UserName,
                    OrderedOn = order.OrderedOn,
                    Product = order.Product.Name
                })
                .ToListAsync();

            return View(models);
        }
    }
}
