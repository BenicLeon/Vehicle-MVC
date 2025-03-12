using AutoMapper;
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
            CreateMap<VehicleModel, VehicleModelDTO>().ReverseMap();
            CreateMap<VehicleMakeDTO, VehicleMakeViewModel>().ReverseMap();
            CreateMap<VehicleModelDTO, VehicleModelViewModel>()
            .ForMember(dest => dest.MakeId, opt => opt.MapFrom(src => src.MakeId))
            .ReverseMap()
            .ForMember(dest => dest.MakeId, opt => opt.MapFrom(src => src.MakeId ?? 0));



        }
    }
}
