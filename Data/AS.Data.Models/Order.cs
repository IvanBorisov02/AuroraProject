using System;

namespace AS.Data.Models
{
    public class Order
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }
        public string OrdererId { get; set; }
        public ASUser Orderer { get; set; }
        public DateTime OrderedOn { get; set; }
    }
}
