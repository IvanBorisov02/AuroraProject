﻿using Microsoft.AspNetCore.Http;

namespace AS.Web.Models
{
    public class ProductEditBindingModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public IFormFile Image { get; set; }
    }
}
