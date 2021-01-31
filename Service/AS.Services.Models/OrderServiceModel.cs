using AS.Data.Models;
using AutoMapperConfiguration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AS.Services.Models
{
    public class OrderServiceModel : IMapTo<Order>, IMapFrom<Order>
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public ProductServiceModel Product { get; set; }
        public string OrdererId { get; set; }
        public ASUserServiceModel Orderer { get; set; }
        public DateTime OrderedOn { get; set; }
    }
}
