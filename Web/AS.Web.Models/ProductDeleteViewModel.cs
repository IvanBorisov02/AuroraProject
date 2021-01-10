﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AS.Web.Models
{
    public class ProductDeleteViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }
}
