using AS.Web.Models.Shared;
using System.Collections.Generic;

namespace AS.Web.Models
{
    public class ProductCategorizedHomeViewModel
    {
        public PagerViewModel Pager { get; set; }

        public ICollection<ProductCategorizedViewModel> Products { get; set; }
    }
}
