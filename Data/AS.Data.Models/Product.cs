using System.ComponentModel.DataAnnotations.Schema;

namespace AS.Data.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public string Description { get; set; }

        public string CategoryId { get; set; }

        public Category Category { get; set; }

        public string ImageUrl { get; set; }

        public int Quantity { get; set; }

        public string GenderTypeId { get; set; }

        public GenderType GenderType { get; set; }

    }
}
