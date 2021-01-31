using AS.Services.Models;
using AutoMapperConfiguration;
using System;

namespace AS.Web.Models
{
    public class OrderAllViewModel : IMapFrom<OrderServiceModel>
    {
        public string Id { get; set; }
        public string Product { get; set; }
        public string Orderer { get; set; }
        public DateTime OrderedOn { get; set; }
    }
}
