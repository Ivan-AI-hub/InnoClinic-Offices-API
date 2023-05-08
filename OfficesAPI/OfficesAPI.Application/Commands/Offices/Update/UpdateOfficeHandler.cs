using AutoMapper;
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
        private IMapper _mapper;
        public UpdateOfficeHandler(IOfficeRepository officeRepository, IMapper mapper, IValidator<UpdateOffice> validator)
        {
            _officeRepository = officeRepository;
            _mapper = mapper;
            _validator = validator;
        }
        public async Task<ApplicationUpdateResult<Office>> Handle(UpdateOffice request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return new ApplicationUpdateResult<Office>(validationResult);

            var office = _mapper.Map<Office>(request);
            var oldOffice = await _officeRepository.GetItemAsync(request.Id, cancellationToken);
            await _officeRepository.UpdateAsync(request.Id, office);
            return new ApplicationUpdateResult<Office>(oldOffice, office);
        }
    }
}
