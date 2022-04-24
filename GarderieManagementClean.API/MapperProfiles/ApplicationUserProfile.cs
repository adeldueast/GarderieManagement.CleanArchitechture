using AutoMapper;
using Contracts.Dtos;
using GarderieManagementClean.Domain.Entities;

namespace GarderieManagementClean.API.MapperProfiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserDTO>().ReverseMap();

            CreateMap<object, ApplicationUserDTO>().ReverseMap();


        }
    }
}
