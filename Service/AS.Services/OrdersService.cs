using AS.Data;
using AS.Data.Models;
using AS.Services.Models;
using AS.Web.Models;
using AutoMapperTestConfiguration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AS.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly ASDbContext db;

        public OrdersService(ASDbContext db)
        {
            this.db = db;
        }

        public async Task<List<OrderServiceModel>> AllOrders()
        {
            List<Order> orders = await this.db.Orders.ToListAsync();

            List<OrderServiceModel> serviceModels = new List<OrderServiceModel>();

            foreach (var order in orders)
            {
                serviceModels.Add(order.To<OrderServiceModel>());
            }

            return serviceModels;
        }


        public async Task<bool> Order(string id, string ordererId)
        {
            Product product = await this.db.Products
              .Include(product => product.Category)
              .SingleOrDefaultAsync(product => product.Id == id);         

            ASUser aSUser = await this.db.Users
                .SingleOrDefaultAsync(user => user.Id == ordererId);

            Order order = new Order
            {
                Id = Guid.NewGuid().ToString(),
                OrderedOn = DateTime.Now,
                Product = product,
                Orderer = aSUser
            };

            bool result = await this.db.AddAsync(order) != null;

            await this.db.SaveChangesAsync();

            return result;

        }
    }
}
