using AutoMapper;
using Default.Api.ViewModels;
using Default.Business.Models;

namespace Default.Api.Configurations
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<User, UserViewModel>().ReverseMap();
        }
    }
}
