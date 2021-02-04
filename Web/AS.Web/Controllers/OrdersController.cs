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
        public async Task<IActionResult> Order(string id, string stripeEmail, string stripeToken)
        {
            var customers = new CustomerService();
            var charges = new ChargeService();

            Data.Models.Product product = await this._context.Products.SingleOrDefaultAsync(product => product.Id == id);


            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = (long)product.Price * 100,
                Currency = "usd",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded")
            {
                string balanceTransactionId = charge.BalanceTransactionId;
            }

            string ordererId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            await this.ordersService.Order(id, ordererId);

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
