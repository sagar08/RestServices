using AutoMapper;
using JwtAuthWithRefreshToken.Data.Entities;
using JwtAuthWithRefreshToken.Models;

namespace JwtAuthWithRefreshToken.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();            
        }
    }
}