﻿using AutoMapper;
using Contracts.Request;
using Contracts.Response;
using GarderieManagementClean.Domain.Entities;

namespace GarderieManagementClean.API.MapperProfiles
{
    public class GarderieProfile : Profile
    {
        public GarderieProfile()
        {
            CreateMap<GarderieRequest, Garderie>().ReverseMap();

            CreateMap<GarderieResponse, Garderie>().ReverseMap();


        }
    }
}
