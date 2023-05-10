using AutoMapper;
using OfficesAPI.Domain;
using OfficesAPI.Services.Models;

namespace OfficesAPI.Services.Mappings
{
    public class ServicesMappingProfile : Profile
    {
        public ServicesMappingProfile()
        {
            CreateMap<Office, OfficeDTO>()
                .ForMember(s => s.Photo, r => r.Ignore())
                .ReverseMap();

            CreateMap<OfficeAddress, OfficeAddressDTO>().ReverseMap();

            CreateMap<CreateOfficeModel, Office>()
            .ForMember(o => o.Address, r => r.MapFrom(x => new OfficeAddress(x.City, x.Street, x.HouseNumber)))
            .ForMember(s => s.Photo, r => r.MapFrom(x => x.Photo.FileName != null ? new Picture(x.Photo.FileName) : null));

            CreateMap<UpdateOfficeModel, Office>()
            .ForMember(o => o.Address, r => r.MapFrom(x => new OfficeAddress(x.City, x.Street, x.HouseNumber)))
            .ForMember(s => s.Photo, r => r.MapFrom(x => x.Photo.FileName != null ? new Picture(x.Photo.FileName) : null));
        }
    }
}
