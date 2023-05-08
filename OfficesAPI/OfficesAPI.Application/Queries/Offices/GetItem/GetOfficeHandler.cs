using MediatR;
using OfficesAPI.Domain;
using OfficesAPI.Domain.Interfaces;

namespace OfficesAPI.Application.Queries.Offices.GetItem
{
    internal class GetOfficeHandler : IRequestHandler<GetOffice, Office?>
    {
        private IOfficeRepository _officeRepository;
        public GetOfficeHandler(IOfficeRepository officeRepository)
        {
            _officeRepository = officeRepository;
        }
        public Task<Office?> Handle(GetOffice request, CancellationToken cancellationToken)
        {
            return _officeRepository.GetItemAsync(request.Id, cancellationToken);
        }
    }
}
