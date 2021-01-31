using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoMapperConfiguration
{
    public interface IHaveCustomMappings
    {
        void CreateMappings(IMapperConfigurationExpression mapperConfig);
    }
}
