using AS.Web.Models.Shared;
using System.Collections.Generic;

namespace AS.Web.Models
{
    public class ProductHomeViewModel
    {
        public PagerViewModel Pager { get; set; }

        public ICollection<ProductViewModel> Products { get; set; }

        public string SearchText { get; set; }
    }
}
