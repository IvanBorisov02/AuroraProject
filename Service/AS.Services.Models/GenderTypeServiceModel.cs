using AS.Data.Models;
using AutoMapperConfiguration;

namespace AS.Services.Models
{
    public class GenderTypeServiceModel : IMapFrom<GenderType>, IMapTo<GenderType>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
