using MediatR;
using OfficesAPI.Domain;

namespace OfficesAPI.Application.Queries.Offices.GetItem
{
    public record GetOffice(Guid Id) : IRequest<Office?>;
}
