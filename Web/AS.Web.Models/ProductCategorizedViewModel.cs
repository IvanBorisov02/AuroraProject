using System;
using System.Collections.Generic;
using System.Text;

namespace AS.Web.Models
{
    public class ProductCategorizedViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string GenderTypeName { get; set; }
        public string ImageUrl { get; set; }

    }
}
