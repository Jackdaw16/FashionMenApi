using AutoMapper;
using fashionMenApi.Models;
using fashionMenApi.Models.ViewModels;

namespace fashionMenApi
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<Order, OrderResponse>();
            CreateMap<Order, OrderDetailResponse>();
            CreateMap<Order, OrderCreate>().ReverseMap();
            
            CreateMap<User, UserResponse>();
            CreateMap<User, UserRegister>().ReverseMap();
            
            CreateMap<Product, ProductResponse>();
        }
    }
}
