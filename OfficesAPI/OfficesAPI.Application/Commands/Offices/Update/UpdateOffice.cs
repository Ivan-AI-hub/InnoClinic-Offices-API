using MediatR;
using OfficesAPI.Application.Results;
using OfficesAPI.Domain;

namespace OfficesAPI.Application.Commands.Offices.Update
{
    public record UpdateOffice(string? PhotoFileName,
                              string City,
                              string Street,
                              int HouseNumber,
                              int OfficeNumber,
                              string PhoneNumber,
                              bool Status) : IRequest<ApplicationUpdateResult<Office>>
    {
        public Guid Id { get; set; }
    }
}
