using AutoMapper;
using Contracts.Dtos.Response;
using GarderieManagementClean.Domain.Entities;

namespace GarderieManagementClean.API.MapperProfiles
{
    public class AttendanceProfile : Profile
    {
        public AttendanceProfile()
        {
            //Id = a.Id,
            //Present = a.ArrivedAt.HasValue ? true : false,
            //Date = a.ArrivedAt.HasValue ? a.ArrivedAt.Value : a.AbsenceDate.Value,
            //AbsenceDescription = a.AbsenceDescription,
            CreateMap<Attendance, AttendanceResponse>()
                .ForMember(dest => dest.Present,
                    opt => opt.MapFrom(src => src.ArrivedAt.HasValue ? true : false))
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => src.ArrivedAt.HasValue ? src.ArrivedAt.Value : src.AbsenceDate.Value));


        }
    }
}
