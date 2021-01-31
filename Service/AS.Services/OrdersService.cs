using AS.Data;
using AS.Data.Models;
using AS.Services.Models;
using AS.Web.Models;
using AutoMapperTestConfiguration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        //public async Task<List<OrderAllViewModel>> AllOrders()
        //{
        //    List<OrderAllViewModel> models = await this.db.Orders
        //       .Include(order => order.Orderer)
        //       .Include(order => order.Product)
        //       .Select(order => new OrderAllViewModel
        //       {
        //           Id = order.Id,
        //           Orderer = order.Orderer.UserName,
        //           OrderedOn = order.OrderedOn,
        //           Product = order.Product.Name
        //       })
        //       .ToListAsync();



        //    return models;
        //}

        public Task<bool> Order(string id)
        {
            throw new NotImplementedException();
        }

        
    }
}
