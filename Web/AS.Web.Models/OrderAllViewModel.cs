using AS.Services.Models;
using AutoMapper;
using AutoMapperConfiguration;
using System;

namespace AS.Web.Models
{
    public class OrderAllViewModel : IMapFrom<OrderServiceModel>, IHaveCustomMappings
    {
        public string Id { get; set; }
        public string Product { get; set; }
        public string Orderer { get; set; }
        public DateTime OrderedOn { get; set; }

        public void CreateMappings(IMapperConfigurationExpression mapperConfig)
        {
            mapperConfig.
              CreateMap<OrderServiceModel, OrderAllViewModel>()
              .ForMember(dest => dest.Product, opts => opts.MapFrom(src => src.Product.Name));
        }
    }
}
