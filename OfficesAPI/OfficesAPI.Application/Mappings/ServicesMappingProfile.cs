using AutoMapper;
using OfficesAPI.Application.Abstraction.Models;
using OfficesAPI.Domain;

namespace OfficesAPI.Application.Mappings
{
    public class ServicesMappingProfile : Profile
    {
        public ServicesMappingProfile()
        {
            CreateMap<Office, OfficeDTO>().ReverseMap();
            CreateMap<Picture, PictureDTO>().ReverseMap();

            CreateMap<OfficeAddress, OfficeAddressDTO>().ReverseMap();

            CreateMap<CreateOfficeModel, Office>()
            .ForMember(o => o.Address, r => r.MapFrom(x => new OfficeAddress(x.City, x.Street, x.HouseNumber)));

            CreateMap<UpdateOfficeModel, Office>()
            .ForMember(o => o.Address, r => r.MapFrom(x => new OfficeAddress(x.City, x.Street, x.HouseNumber)));
        }
    }
}
