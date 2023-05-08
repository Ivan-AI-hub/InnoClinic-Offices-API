using MediatR;
using OfficesAPI.Domain;

namespace OfficesAPI.Application.Queries.Offices.GetPage
{
    public record GetOfficesPage(int PageNumber, int PageSize) : IRequest<IEnumerable<Office>>;
}
