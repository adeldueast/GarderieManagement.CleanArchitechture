using AutoMapper;
using Contracts.Dtos.Request;
using Contracts.Dtos.Response;
using GarderieManagementClean.Domain.Entities;

namespace GarderieManagementClean.API.MapperProfiles
{
    public class JournalProfile : Profile
    {

        public JournalProfile()
        {
            CreateMap<JournalCreateRequest, JournalDeBord>();

            CreateMap<JournalUpdateRequest, JournalDeBord>();

            CreateMap<JournalDeBord, JournalResponse>()
                .ForMember(dest => dest.CreatedBy, opt=> opt.MapFrom(src=>src.ApplicationUser.Email));

        }
    }
}
