using FluentValidation;
using MediatR;
using OfficesAPI.Application.Interfaces;
using OfficesAPI.Domain;

namespace OfficesAPI.Application.Commands.Offices.Create
{
    internal class CreateOfficeHandler : IRequestHandler<CreateOffice, ApplicationValueResult<Office>>
    {
        private IOfficeRepository _officeRepository;
        private IValidator<CreateOffice> _validator;
        public CreateOfficeHandler(IOfficeRepository officeRepository, IValidator<CreateOffice> validator)
        {
            _officeRepository = officeRepository;
            _validator = validator;
        }

        public async Task<ApplicationValueResult<Office>> Handle(CreateOffice request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return new ApplicationValueResult<Office>(validationResult);

            var isOfficeNumberInvalid = await _officeRepository.IsItemExistAsync(x => x.Address.City == request.City &&
                                                                                     x.OfficeNumber == request.OfficeNumber);

            if(isOfficeNumberInvalid)
                return new ApplicationValueResult<Office>(null, $"There is already an office at {request.OfficeNumber} in {request.City}");

            var address = new OfficeAddress(request.City, request.Street, request.HouseNumber);

            var picture = request.PhotoFileName != null? new Picture(request.PhotoFileName): null;

            var office = new Office(address, request.OfficeNumber, request.PhoneNumber, request.Status, picture);

            await _officeRepository.CreateAsync(office);
            return new ApplicationValueResult<Office>(office);
        }
    }
}
