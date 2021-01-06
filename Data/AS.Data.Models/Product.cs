namespace AS.Data.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

        public string CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
