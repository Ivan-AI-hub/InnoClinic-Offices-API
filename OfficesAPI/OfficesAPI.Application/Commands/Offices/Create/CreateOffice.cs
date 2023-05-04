using MediatR;
using Microsoft.AspNetCore.Http;
using OfficesAPI.Domain;

namespace OfficesAPI.Application.Commands.Offices.Create
{
   public record CreateOffice(IFormFile Photo,
                              string City,
                              string Street,
                              int HouseNumber,
                              int OfficeNumber,
                              string PhoneNumber,
                              bool Status) : IRequest<ApplicationValueResult<Office>>;
}
