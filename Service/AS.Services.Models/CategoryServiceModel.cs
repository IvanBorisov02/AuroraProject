using AS.Data.Models;
using AutoMapperConfiguration;

namespace AS.Services.Models
{
    public class CategoryServiceModel : IMapFrom<Category>, IMapTo<Category>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
