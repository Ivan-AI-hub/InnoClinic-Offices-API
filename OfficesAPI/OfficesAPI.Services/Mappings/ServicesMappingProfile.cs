using AutoMapper;
using OfficesAPI.Application.Commands.Offices.Create;
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
            CreateMap<CreateOfficeModel, CreateOffice>()
                .ForMember(s => s.PhotoFileName, r => r.MapFrom(d => d.Photo.Name));
        }
    }
}
