using FluentValidation;
using MediatR;
using OfficesAPI.Domain.Exceptions;
using OfficesAPI.Domain.Interfaces;

namespace OfficesAPI.Application.Commands.Offices.UpdateStatus
{
    internal class UpdateOfficeStatusHandler : IRequestHandler<UpdateOfficeStatus>
    {
        private IOfficeRepository _officeRepository;
        private IValidator<UpdateOfficeStatus> _validator;
        public UpdateOfficeStatusHandler(IOfficeRepository officeRepository, IValidator<UpdateOfficeStatus> validator)
        {
            _officeRepository = officeRepository;
            _validator = validator;
        }
        public async Task Handle(UpdateOfficeStatus request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var office = await _officeRepository.GetItemAsync(request.Id, cancellationToken);

            if (office == null)
                throw new OfficeNotFoundException(request.Id);

            if (office.Status == request.NewStatus)
                throw new OfficeHaveThatStatusException(request.NewStatus);

            office.Status = request.NewStatus;

            await _officeRepository.UpdateAsync(request.Id, office);
        }
    }
}
