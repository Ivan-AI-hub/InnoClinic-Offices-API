using AutoMapper;
using FluentValidation;
using MediatR;
using OfficesAPI.Application.Results;
using OfficesAPI.Domain;
using OfficesAPI.Domain.Interfaces;

namespace OfficesAPI.Application.Commands.Offices.Create
{
    internal class CreateOfficeHandler : IRequestHandler<CreateOffice, ApplicationValueResult<Office>>
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

        public async Task<ApplicationValueResult<Office>> Handle(CreateOffice request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return new ApplicationValueResult<Office>(validationResult);

            var office = _mapper.Map<Office>(request);

            await _officeRepository.CreateAsync(office);
            return new ApplicationValueResult<Office>(office);
        }
    }
}
