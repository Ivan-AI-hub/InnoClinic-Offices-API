using FluentValidation;
using MediatR;
using OfficesAPI.Application.Interfaces;
using OfficesAPI.Domain;

namespace OfficesAPI.Application.Commands.Offices.Create
{
    internal class CreateOfficeHandler : IRequestHandler<CreateOffice, ApplicationValueResult<Office>>
    {
        private IRepositoryManager _repositoryManager;
        private IBlobManager _blobManager;
        private IValidator<CreateOffice> _validator;
        public CreateOfficeHandler(IRepositoryManager repositoryManager, IValidator<CreateOffice> validator, IBlobManager blobManager)
        {
            _repositoryManager = repositoryManager;
            _validator = validator;
            _blobManager = blobManager;
        }

        public async Task<ApplicationValueResult<Office>> Handle(CreateOffice request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return new ApplicationValueResult<Office>(validationResult);


            var isOfficeNumberInvalid = await _repositoryManager.OfficeRepository
                                                              .IsItemExistAsync(x => x.Address.City == request.City &&
                                                                                     x.OfficeNumber == request.OfficeNumber);

            if(isOfficeNumberInvalid)
                return new ApplicationValueResult<Office>(null, $"There is already an office at {request.OfficeNumber} in {request.City}");

            await _blobManager.UploadAsync(request.Photo, cancellationToken);

            var address = new OfficeAddress(request.City, request.Street, request.HouseNumber);
            var picture = new Picture(request.Photo.FileName);
            var office = new Office(address, request.OfficeNumber, request.PhoneNumber, request.Status, picture);
            _repositoryManager.OfficeRepository.Create(office);
            await _repositoryManager.SaveChangesAsync(cancellationToken);
            return new ApplicationValueResult<Office>(office);
        }
    }
}
