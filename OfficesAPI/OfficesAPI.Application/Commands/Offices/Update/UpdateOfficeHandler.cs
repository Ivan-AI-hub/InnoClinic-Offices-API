using AutoMapper;
using FluentValidation;
using MediatR;

using OfficesAPI.Domain;
using OfficesAPI.Domain.Exceptions;
using OfficesAPI.Domain.Interfaces;

namespace OfficesAPI.Application.Commands.Offices.Update
{
    internal class UpdateOfficeHandler : IRequestHandler<UpdateOffice, Office>
    {
        private IOfficeRepository _officeRepository;
        private IValidator<UpdateOffice> _validator;
        private IMapper _mapper;
        public UpdateOfficeHandler(IOfficeRepository officeRepository, IMapper mapper, IValidator<UpdateOffice> validator)
        {
            _officeRepository = officeRepository;
            _mapper = mapper;
            _validator = validator;
        }
        public async Task<Office> Handle(UpdateOffice request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
               throw new ValidationException(validationResult.Errors);

            var office = _mapper.Map<Office>(request);
            var oldOffice = await _officeRepository.GetItemAsync(request.Id, cancellationToken);

            if (oldOffice == null)
                throw new OfficeNotFoundException(request.Id);

            if (oldOffice.Address.City != request.City || oldOffice.OfficeNumber != request.OfficeNumber)
            {
                var isOfficeNumberInvalid = await _officeRepository.IsItemExistAsync(x => x.Address.City == request.City 
                                                                                    && x.OfficeNumber == request.OfficeNumber);

                if (isOfficeNumberInvalid)
                    throw new CityAlreadyHaveOfficeWithThatNumberException(request.City, request.OfficeNumber);
            }

            await _officeRepository.UpdateAsync(request.Id, office);
            return oldOffice;
        }
    }
}
