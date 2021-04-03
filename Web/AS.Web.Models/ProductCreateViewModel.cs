using AS.Services.Models;
using AutoMapper;
using AutoMapperConfiguration;
using Microsoft.AspNetCore.Http;
using System;

namespace AS.Web.Models
{
    public class ProductCreateViewModel : IMapTo<ProductServiceModel>, IHaveCustomMappings
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }
        
        public IFormFile Image { get; set; }

        public int Quantity { get; set; }

        public string GenderType { get; set; }

        public void CreateMappings(IMapperConfigurationExpression mapperConfig)
        {
            mapperConfig.
              CreateMap<ProductCreateViewModel, ProductServiceModel>()
              .ForMember(dest => dest.CategoryServiceModel, opts => opts.MapFrom(src => new CategoryServiceModel { Name = src.Category }))
              .ForMember(dest => dest.GenderTypeServiceModel, opts => opts.MapFrom(src => new GenderTypeServiceModel { Name = src.GenderType}));
           
        }
    }
}
