using AutoMapper;
using Contracts.Dtos;
using GarderieManagementClean.Domain.Entities;

namespace GarderieManagementClean.API.MapperProfiles
{
    public class TutorEnfantProfile : Profile
    {
        public TutorEnfantProfile()
        {
            CreateMap<TutorEnfant, TutorEnfantDTO>().ReverseMap();
        }
    }
}
