﻿using AutoMapper;
using Vehicle.Common.ViewModels;
using Vehicle.Service.DTOs;
using Vehicle.Service.Models;


namespace Vehicle.Service.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<VehicleMake, VehicleMakeDTO>().ReverseMap();
            // CreateMap<VehicleModel, VehicleModelDTO>().ReverseMap();
            CreateMap<VehicleMakeDTO, VehicleMakeViewModel>().ReverseMap();



        }
    }
}
