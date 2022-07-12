using AutoMapper;
using Contracts.Dtos.Response;
using GarderieManagementClean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GarderieManagementClean.API.MapperProfiles
{
    public class EnfantProfile : Profile
    {
        public EnfantProfile()
        {
            CreateMap<Enfant, EnfantResponse>().ReverseMap();
            CreateMap<object, EnfantResponse>().ReverseMap();

            CreateMap<Enfant, EnfantDetailResponse>().ReverseMap();

          
            CreateMap<Enfant, EnfantSummariesResponse>()
                .ForMember(dest => dest.hasArrived,
                    opt => opt.MapFrom(src => src.Attendances.Any(attendance=> attendance.ArrivedAt.HasValue && attendance.ArrivedAt.Value.Date == DateTime.Now.Date )));


        }
    }
}
