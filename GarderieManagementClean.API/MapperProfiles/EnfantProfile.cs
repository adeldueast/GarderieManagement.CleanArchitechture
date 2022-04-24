using AutoMapper;
using Contracts.Dtos.Response;
using GarderieManagementClean.Domain.Entities;
using System.Collections.Generic;

namespace GarderieManagementClean.API.MapperProfiles
{
    public class EnfantProfile : Profile
    {
        public EnfantProfile()
        {
            CreateMap<Enfant, EnfantResponse>().ReverseMap();
            CreateMap<object, EnfantResponse>().ReverseMap();

            CreateMap<Enfant, EnfantDetailResponse>().ReverseMap();


        }
    }
}
