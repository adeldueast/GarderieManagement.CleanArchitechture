using AutoMapper;
using Contracts.Dtos;
using GarderieManagementClean.Domain.Entities;

namespace GarderieManagementClean.API.MapperProfiles
{
    public class AddresseProfile : Profile
    {
        public AddresseProfile()
        {
            CreateMap<Address, AddressDTO>().ReverseMap();

        }

    }
}
