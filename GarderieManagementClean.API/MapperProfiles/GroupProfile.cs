using AutoMapper;
using Contracts.Dtos;
using Contracts.Dtos.Request;
using Contracts.Dtos.Response;
using Contracts.Request;
using Contracts.Response;
using GarderieManagementClean.Domain.Entities;
using System;
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
                       opt => opt.MapFrom(src => $"{src.ApplicationUser.FirstName} {src.ApplicationUser.LastName}"))
            .ForMember(dest => dest.Enfants,
                       opt => opt.MapFrom(src => src.Enfants.Select(e => new
                       {
                           Id = e.Id,
                           Nom = e.Nom,
                           Image = e.PhotoCouverture!= null ? e.PhotoCouverture.Id.ToString() : null,
                           hasArrived = e.Attendances.Any(attendance => !attendance.ArrivedAt.HasValue ? false :  attendance.ArrivedAt.Value.Date == DateTime.Now.Date && !attendance.LeftAt.HasValue),

                       }).ToList())
                       );

            CreateMap<GroupResponse, object>();
            CreateMap<object, GroupResponse>();

        }
    }
}
