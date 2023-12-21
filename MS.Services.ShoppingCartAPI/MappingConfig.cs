using AutoMapper;
using MS.Services.ShoppingCartAPI.Models;
using MS.Services.ShoppingCartAPI.Models.Dto;

namespace MS.Services.ShoppingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeaderDto, CartHeader>().ReverseMap();
                config.CreateMap<CartDetailsDto, CartDetails>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
