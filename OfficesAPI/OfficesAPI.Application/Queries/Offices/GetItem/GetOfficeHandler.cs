using MediatR;
using OfficesAPI.Domain;
using OfficesAPI.Domain.Exceptions;
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
        public async Task<Office> Handle(GetOffice request, CancellationToken cancellationToken)
        {
            var office = await _officeRepository.GetItemAsync(request.Id, cancellationToken);
            if (office == null)
                throw new OfficeNotFoundException(request.Id);
            return office;
        }
    }
}
