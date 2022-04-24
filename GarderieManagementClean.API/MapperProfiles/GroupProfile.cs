using AutoMapper;
using Contracts.Dtos;
using Contracts.Dtos.Request;
using Contracts.Dtos.Response;
using Contracts.Request;
using Contracts.Response;
using GarderieManagementClean.Domain.Entities;

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
            CreateMap<Group, GroupResponse>();
            CreateMap<GroupResponse, object>();
            CreateMap<object, GroupResponse>();

        }
    }
}
