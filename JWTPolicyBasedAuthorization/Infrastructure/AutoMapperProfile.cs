using AutoMapper;
using JWTPolicyBasedAuthorization.Dtos;
using JWTPolicyBasedAuthorization.Models;

namespace JWTPolicyBasedAuthorization.Infrastructure
{

    public class AutoMapperProfile : Profile
    {

        #region  Constructor
        public AutoMapperProfile()
        {
            ProdcutMapper();

        }

        #endregion

        #region Private Methods

        private void ProdcutMapper()
        {
            CreateMap<Product, ProductDetailDto>()
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.Value))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Value));
        }
        #endregion
    }
}