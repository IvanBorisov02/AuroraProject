using AS.Services.Models;
using AutoMapperConfiguration;
using Microsoft.AspNetCore.Http;
using System;

namespace AS.Web.Models
{
    public class ProductCreateViewModel : IMapTo<ProductServiceModel>
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }
        
        public IFormFile Image { get; set; }
    }
}
