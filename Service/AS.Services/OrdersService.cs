using AS.Data;
using AS.Data.Models;
using AS.Services.Models;
using AS.Web.Models;
using AutoMapperTestConfiguration;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Korzh.EasyQuery.Linq;

namespace AS.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly ASDbContext db;
        private readonly IProductsService productsService;

        public OrdersService(ASDbContext db, IProductsService productsService)
        {
            this.db = db;
            this.productsService = productsService;
        }

        public async Task<List<OrderServiceModel>> AllOrders(string searchText)
        {
            List<Data.Models.Order> orders = new List<Data.Models.Order>();
            orders = await this.db.Orders.ToListAsync();


            List<OrderServiceModel> serviceModels = new List<OrderServiceModel>();

            foreach (var order in orders)
            {
                var currentProduct = await this.db.Products.FirstOrDefaultAsync(product => product.Id == order.ProductId);
                var currentOrderer = await this.db.Users.FirstOrDefaultAsync(user => user.Id == order.OrdererId);

                order.Product = currentProduct;
                order.Orderer = currentOrderer;

                serviceModels.Add(order.To<OrderServiceModel>());

            }


            List<OrderServiceModel> result = new List<OrderServiceModel>();
            if (!string.IsNullOrEmpty(searchText))
            {
                result = serviceModels.Where(x => x.Product.Name.Contains(searchText) || x.Orderer.Email.Contains(searchText) || x.OrderedOn.ToString("MM dd yyyy HH:mm:ss").Contains(searchText)).ToList();
            }
            else
            {
                result = serviceModels;
            }

            return result;
        }


        public async Task<bool> Order(string id, string ordererId, string stripeEmail, string stripeToken, int itemCount)
        {

            var customers = new CustomerService();
            var charges = new ChargeService();          

            Data.Models.Product product = await this.db.Products
              .Include(product => product.Category)
              .SingleOrDefaultAsync(product => product.Id == id);

            if (product.Quantity <= 0)
            {
                throw new ArgumentException("We don't have such amount of this product");
            }

            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = (long)product.Price * 100 * itemCount,
                Currency = "usd",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded")
            {
                string balanceTransactionId = charge.BalanceTransactionId;
            }                  

            ASUser aSUser = await this.db.Users
                .SingleOrDefaultAsync(user => user.Id == ordererId);

            Data.Models.Order order = new Data.Models.Order
            {
                Id = Guid.NewGuid().ToString(),
                OrderedOn = DateTime.Now,
                Product = product,
                Orderer = aSUser
            };

            await productsService.DecreaseQuantity(product.Id, itemCount);

            bool result = await this.db.AddAsync(order) != null;

            await this.db.SaveChangesAsync();

            return result;

        }
    }
}
