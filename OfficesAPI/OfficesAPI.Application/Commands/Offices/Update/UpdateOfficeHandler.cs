using FluentValidation;
using MediatR;
using OfficesAPI.Application.Results;
using OfficesAPI.Domain;
using OfficesAPI.Domain.Interfaces;

namespace OfficesAPI.Application.Commands.Offices.Update
{
    internal class UpdateOfficeHandler : IRequestHandler<UpdateOffice, ApplicationUpdateResult<Office>>
    {
        private IOfficeRepository _officeRepository;
        private IValidator<UpdateOffice> _validator;
        public UpdateOfficeHandler(IOfficeRepository officeRepository, IValidator<UpdateOffice> validator)
        {
            _officeRepository = officeRepository;
            _validator = validator;
        }
        public async Task<ApplicationUpdateResult<Office>> Handle(UpdateOffice request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return new ApplicationUpdateResult<Office>(validationResult);

            var oldOffice = await _officeRepository.GetItemAsync(request.Id, cancellationToken);
            if (oldOffice.Address.City != request.City || oldOffice.OfficeNumber != request.OfficeNumber)
            {
                var isOfficeNumberInvalid = await _officeRepository.IsItemExistAsync(x => x.Address.City == request.City &&
                                                                         x.OfficeNumber == request.OfficeNumber, cancellationToken);

                if (isOfficeNumberInvalid)
                    return new ApplicationUpdateResult<Office>(errors: $"There is already an office at {request.OfficeNumber} in {request.City}");
            }

            var address = new OfficeAddress(request.City, request.Street, request.HouseNumber);

            var picture = request.PhotoFileName != null ? new Picture(request.PhotoFileName) : null;

            var office = new Office(request.Id, address, request.OfficeNumber, request.PhoneNumber, request.Status, picture);

            await _officeRepository.UpdateAsync(request.Id, office);
            return new ApplicationUpdateResult<Office>(oldOffice, office);
        }
    }
}
