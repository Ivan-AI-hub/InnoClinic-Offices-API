using FluentValidation;
using MediatR;
using OfficesAPI.Application.Results;
using OfficesAPI.Domain.Interfaces;

namespace OfficesAPI.Application.Commands.Offices.UpdateStatus
{
    internal class UpdateOfficeStatusHandler : IRequestHandler<UpdateOfficeStatus, ApplicationVoidResult>
    {
        private IOfficeRepository _officeRepository;
        private IValidator<UpdateOfficeStatus> _validator;
        public UpdateOfficeStatusHandler(IOfficeRepository officeRepository, IValidator<UpdateOfficeStatus> validator)
        {
            _officeRepository = officeRepository;
            _validator = validator;
        }
        public async Task<ApplicationVoidResult> Handle(UpdateOfficeStatus request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return new ApplicationVoidResult(validationResult);

            var office = await _officeRepository.GetItemAsync(request.Id, cancellationToken);

            office.Status = request.NewStatus;

            await _officeRepository.UpdateAsync(request.Id, office);
            return new ApplicationVoidResult();
        }
    }
}
