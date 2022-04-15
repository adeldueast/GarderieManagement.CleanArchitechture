using AutoMapper;
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

            CreateMap<GroupRequest, Group>().ReverseMap();

            CreateMap<GroupResponse, Group>().ReverseMap();
        }
    }
}
