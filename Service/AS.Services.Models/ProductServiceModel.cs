using AS.Data.Models;
using AutoMapperConfiguration;
using System;

namespace AS.Services.Models
{
    public class ProductServiceModel : IMapFrom<Product>, IMapTo<Product>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string CategoryId { get; set; }

        public CategoryServiceModel CategoryServiceModel { get; set; }

        public string ImageUrl { get; set; }
    }
}
