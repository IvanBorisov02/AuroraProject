using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoMapperConfiguration
{
    public class AutoMapperConfig
    {
        public static IMapper Mapper;

        public static void RegisterMappings(params Type[][] typeCollections)
        {

            MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
            {
                foreach (var typeCollection in typeCollections)
                {
                    foreach (var type in typeCollection)
                    {
                        List<Type> mapFromTypes = type.GetInterfaces()
                            .Where(i => i.Name.Contains("IMapFrom"))
                            .Select(i => i.GetGenericArguments()[0])
                            .ToList();

                        foreach (var fromType in mapFromTypes)
                        {
                            mapperConfig.CreateMap(fromType, type);
                        }

                        List<Type> mapToTypes = type.GetInterfaces()
                            .Where(i => i.Name.Contains("IMapTo"))
                            .Select(i => i.GetGenericArguments()[0])
                            .ToList();

                        foreach (var toType in mapToTypes)
                        {
                            mapperConfig.CreateMap(type, toType);
                        }

                    }
                }
            });

            Mapper = mapperConfiguration.CreateMapper();
        }
    }
}
