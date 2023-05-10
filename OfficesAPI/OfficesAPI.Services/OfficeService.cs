using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using OfficesAPI.Domain;
using OfficesAPI.Domain.Exceptions;
using OfficesAPI.Domain.Interfaces;
using OfficesAPI.Services.Abstraction;
using OfficesAPI.Services.Abstraction.Models;
using OfficesAPI.Services.Exceptions;
using OfficesAPI.Services.Validators;

namespace OfficesAPI.Services
{
    public class OfficeService : IOfficeService
    {
        private IMapper _mapper;
        private IOfficeRepository _officeRepository;
        private IBlobService _blobService;
        private IValidator<CreateOfficeModel> _createOfficeValidator;
        private IValidator<UpdateOfficeModel> _updateOfficeValidator;

        public OfficeService(IMapper mapper, IOfficeRepository officeRepository, IBlobService blobService,
            IValidator<CreateOfficeModel> createOfficeValidator, IValidator<UpdateOfficeModel> updateOfficeValidator)
        {
            _mapper = mapper;
            _officeRepository = officeRepository;
            _blobService = blobService;
            _createOfficeValidator = createOfficeValidator;
            _updateOfficeValidator = updateOfficeValidator;
        }

        public async Task<OfficeDTO> CreateAsync(CreateOfficeModel model, CancellationToken cancellationToken = default)
        {
            await ValidateBlobFileName(model.Photo, cancellationToken);
            await ValidateModel(model, _createOfficeValidator, cancellationToken);
            await ValidateOfficeNumber(model.City, model.OfficeNumber, cancellationToken);

            var office = _mapper.Map<Office>(model);
            await _officeRepository.CreateAsync(office);

            if (model.Photo != null)
                await _blobService.UploadAsync(model.Photo, cancellationToken);

            return await GetOfficeDTOWithPhotoAsync(office);
        }

        public async Task UpdateAsync(Guid id, UpdateOfficeModel model, CancellationToken cancellationToken = default)
        {
            await ValidateBlobFileName(model.Photo, cancellationToken);
            await ValidateModel(model, _updateOfficeValidator, cancellationToken);

            var oldOffice = await _officeRepository.GetItemAsync(id, cancellationToken);

            if (oldOffice == null)
                throw new OfficeNotFoundException(id);

            if (oldOffice.Address.City != model.City || oldOffice.OfficeNumber != model.OfficeNumber)
                await ValidateOfficeNumber(model.City, model.OfficeNumber);

            var office = _mapper.Map<Office>(model);
            office.Id = id;
            await _officeRepository.UpdateAsync(id, office);

            if (oldOffice.Photo != null && await _blobService.IsBlobExist(oldOffice.Photo.Name))
                await _blobService.DeleteAsync(oldOffice.Photo.Name);

            if (model.Photo != null)
                await _blobService.UploadAsync(model.Photo);
        }

        public async Task UpdateStatus(Guid id, bool newStatus, CancellationToken cancellationToken = default)
        {
            var office = await _officeRepository.GetItemAsync(id, cancellationToken);

            if (office == null)
                throw new OfficeNotFoundException(id);

            if (office.Status == newStatus)
                throw new OfficeHaveThatStatusException(newStatus);

            office.Status = newStatus;
            await _officeRepository.UpdateAsync(id, office);
        }

        public IEnumerable<OfficeDTO> GetOfficesPage(int pageNumber, int pageSize)
        {
            var offices = _officeRepository.GetItems();
            offices = offices.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            return _mapper.Map<IEnumerable<OfficeDTO>>(offices);
        }

        public async Task<OfficeDTO> GetOfficeAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var office = await _officeRepository.GetItemAsync(id, cancellationToken);
            if (office == null)
                throw new OfficeNotFoundException(id);

            return await GetOfficeDTOWithPhotoAsync(office);
        }

        private async Task<OfficeDTO> GetOfficeDTOWithPhotoAsync(Office officeData, CancellationToken cancellationToken = default)
        {
            var office = _mapper.Map<OfficeDTO>(officeData);

            if (officeData.Photo != null)
                office.Photo = await _blobService.DownloadAsync(officeData.Photo.Name, cancellationToken);

            return office;
        }

        private async Task ValidateBlobFileName(IFormFile? file, CancellationToken cancellationToken = default)
        {
            if (file != null && await _blobService.IsBlobExist(file.FileName, cancellationToken))
                throw new BlobNameIsNotValidException(file.FileName);
        }

        private async Task ValidateOfficeNumber(string city, int officeNumber, CancellationToken cancellationToken = default)
        {
            var isOfficeNumberInvalid = await _officeRepository.IsItemExistAsync(x => x.Address.City == city &&
                                                                                      x.OfficeNumber == officeNumber,
                                                                                      cancellationToken);

            if (isOfficeNumberInvalid)
                throw new CityAlreadyHaveOfficeWithThatNumberException(city, officeNumber);
        }

        private async Task ValidateModel<Tmodel>(Tmodel model, IValidator<Tmodel> validator, CancellationToken cancellationToken = default)
        {
            var validationResult = await validator.ValidateAsync(model, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }
    }
}
