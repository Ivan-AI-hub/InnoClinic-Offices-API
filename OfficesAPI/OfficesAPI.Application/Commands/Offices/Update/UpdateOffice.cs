using MediatR;

using OfficesAPI.Domain;

namespace OfficesAPI.Application.Commands.Offices.Update
{
    public record UpdateOffice(string? PhotoFileName,
                              string City,
                              string Street,
                              int HouseNumber,
                              int OfficeNumber,
                              string PhoneNumber,
                              bool Status) : IRequest<Office>
    {
        public Guid Id { get; set; }
    }
}
