using AutoMapper;
using CarsPro.Data.DTO_s;
using CarsPro.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsPro.Data.Mapping
{
    public class Map : Profile
    {
        public Map()
        {
            CreateMap<Car, CarDTO>().ReverseMap();
            CreateMap<Factory, FactoryDTO>().ReverseMap();
        }
    }
}
