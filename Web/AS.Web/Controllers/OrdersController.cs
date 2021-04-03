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
using AS.Services;
using AS.Services.Models;
using AutoMapperTestConfiguration;
using Stripe;

namespace AS.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ASDbContext _context;
        private readonly IOrdersService ordersService;

        public OrdersController(ASDbContext context, IOrdersService ordersService)
        {
            _context = context;
            this.ordersService = ordersService;
        }

        [HttpPost]
        public async Task<IActionResult> Order(string id, string stripeEmail, string stripeToken, ProductDetailsViewModel model)
        {           
            string ordererId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            await this.ordersService.Order(id, ordererId, stripeEmail, stripeToken, model.OrderedCount);

            return Redirect($"/Products/Details/{id}");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllAsync()
        {
            List<OrderAllViewModel> models = new List<OrderAllViewModel>();
            List<OrderServiceModel> serviceModels = await this.ordersService.AllOrders();

            foreach (var serviceModel in serviceModels)
            {
               
                models.Add(serviceModel.To<OrderAllViewModel>());
            }

            return View(models);
        }
    }
}
