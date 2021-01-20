using System;
using System.Collections.Generic;
using System.Text;

namespace AS.Web.Models
{
    public class ProductEditViewModel : ProductCreateViewModel
    {
        public string Id { get; set; }

        public string ImageUrl { get; set; }
    }
}
