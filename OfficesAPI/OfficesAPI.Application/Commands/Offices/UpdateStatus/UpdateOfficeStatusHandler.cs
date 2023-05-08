using MediatR;
using OfficesAPI.Application.Results;
using OfficesAPI.Domain.Interfaces;

namespace OfficesAPI.Application.Commands.Offices.UpdateStatus
{
    internal class UpdateOfficeStatusHandler : IRequestHandler<UpdateOfficeStatus, ApplicationVoidResult>
    {
        private IOfficeRepository _officeRepository;
        public UpdateOfficeStatusHandler(IOfficeRepository officeRepository)
        {
            _officeRepository = officeRepository;
        }
        public async Task<ApplicationVoidResult> Handle(UpdateOfficeStatus request, CancellationToken cancellationToken)
        {
            var office = await _officeRepository.GetItemAsync(request.Id, cancellationToken);
            if (office.Status == request.NewStatus)
                return new ApplicationVoidResult($"Office already have status {office.Status}");

            office.Status = request.NewStatus;
            await _officeRepository.UpdateAsync(request.Id, office);
            return new ApplicationVoidResult();
        }
    }
}
