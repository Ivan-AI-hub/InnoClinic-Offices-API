using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficesAPI.Domain;
using OfficesAPI.Domain.Interfaces;

namespace OfficesAPI.Application.Queries.Offices.GetPage
{
    internal class GetOfficesPageHandler : IRequestHandler<GetOfficesPage, IEnumerable<Office>>
    {
        private IOfficeRepository _officeRepository;
        public GetOfficesPageHandler(IOfficeRepository officeRepository) 
        {
            _officeRepository = officeRepository;
        }

        public Task<IEnumerable<Office>> Handle(GetOfficesPage request, CancellationToken cancellationToken)
        {
            var offices = _officeRepository.GetItems();
            return Task.FromResult(offices.Skip(request.PageSize * (request.PageNumber - 1)).Take(request.PageSize).AsEnumerable());
        }
    }
}
