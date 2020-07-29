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
            ProdcutDetailMapper();
        }

        #endregion

        #region Private Methods

        private void ProdcutMapper()
        {
            CreateMap<ProductDto, Product>();
        }

        private void ProdcutDetailMapper()
        {
            CreateMap<Product,ProductDetailDto>()
            .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Value))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Value));
        }

        #endregion
    }
}