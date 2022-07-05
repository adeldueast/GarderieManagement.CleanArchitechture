using AutoMapper;
using Contracts.Dtos;
using Contracts.Dtos.Request;
using Contracts.Dtos.Response;
using Contracts.Request;
using Contracts.Response;
using GarderieManagementClean.Domain.Entities;
using System.Linq;

namespace GarderieManagementClean.API.MapperProfiles
{
    public class GroupProfile : Profile
    {

        public GroupProfile()
        {

            CreateMap<GroupCreateRequest, Group>().ReverseMap();

            CreateMap<GroupDTO, Group>().ReverseMap();

            CreateMap<GroupUpdateRequest, Group>().ReverseMap();

            CreateMap<GroupResponse, Group>();

            CreateMap<Group, GroupResponse>()
            .ForMember(dest => dest.EducatriceFullName,
                       opt => opt.MapFrom(src => $"{src.ApplicationUser.FirstName} {src.ApplicationUser.LastName}")
                       )
            .ForMember(dest => dest.EnfantsIds,
                       opt => opt.MapFrom(src => src.Enfants.Select(e => e.Id).ToList())
                       );

            CreateMap<GroupResponse, object>();
            CreateMap<object, GroupResponse>();

        }
    }
}
