using AutoMapper;
using FluentValidation;
using MediatR;

using OfficesAPI.Domain;
using OfficesAPI.Domain.Exceptions;
using OfficesAPI.Domain.Interfaces;

namespace OfficesAPI.Application.Commands.Offices.Create
{
    internal class CreateOfficeHandler : IRequestHandler<CreateOffice, Office>
    {
        private IOfficeRepository _officeRepository;
        private IValidator<CreateOffice> _validator;
        private IMapper _mapper;
        public CreateOfficeHandler(IOfficeRepository officeRepository, IMapper mapper, IValidator<CreateOffice> validator)
        {
            _officeRepository = officeRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<Office> Handle(CreateOffice request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var isOfficeNumberInvalid = await _officeRepository.IsItemExistAsync(x => x.Address.City == request.City &&
                                                                                 x.OfficeNumber == request.OfficeNumber);

            if (isOfficeNumberInvalid)
                throw new CityAlreadyHaveOfficeWithThatNumberException(request.City, request.OfficeNumber);

            var office = _mapper.Map<Office>(request);

            await _officeRepository.CreateAsync(office);
            return office;
        }
    }
}
