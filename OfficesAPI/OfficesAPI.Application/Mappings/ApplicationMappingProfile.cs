using AutoMapper;
using OfficesAPI.Application.Commands.Offices.Create;
using OfficesAPI.Application.Commands.Offices.Update;
using OfficesAPI.Domain;

namespace OfficesAPI.Application.Mappings
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<CreateOffice, Office>()
            .ForMember(o => o.Address, r => r.MapFrom(x => new OfficeAddress(x.City, x.Street, x.HouseNumber)))
            .ForMember(s => s.Photo, r => r.MapFrom(x => x.PhotoFileName != null ? new Picture(x.PhotoFileName) : null));

            CreateMap<UpdateOffice, Office>()
            .ForMember(o => o.Address, r => r.MapFrom(x => new OfficeAddress(x.City, x.Street, x.HouseNumber)))
            .ForMember(s => s.Photo, r => r.MapFrom(x => x.PhotoFileName != null ? new Picture(x.PhotoFileName) : null));
        }
    }
}
