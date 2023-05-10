using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using OfficesAPI.Domain;
using OfficesAPI.Domain.Exceptions;
using OfficesAPI.Domain.Interfaces;
using OfficesAPI.Services.Models;
using OfficesAPI.Services.Validators;

namespace OfficesAPI.Services
{
    public class OfficeService
    {
        private IMapper _mapper;
        private IOfficeRepository _officeRepository;
        private BlobService _blobService;

        public OfficeService(IMapper mapper, IOfficeRepository officeRepository, BlobService blobService)
        {
            _mapper = mapper;
            _officeRepository = officeRepository;
            _blobService = blobService;
        }

        /// <summary>
        /// Creates the office in the system
        /// </summary>
        /// <param name="model">Model for creating office</param>
        public async Task<OfficeDTO> CreateAsync(CreateOfficeModel model, CancellationToken cancellationToken = default)
        {
            await IsBlobFileNameValid(model.Photo, cancellationToken);
            await IsModelValid(model, new CreateOfficeValidator(), cancellationToken);
            await IsOfficeNumberValid(model.City, model.OfficeNumber, cancellationToken);

            var office = _mapper.Map<Office>(model);
            await _officeRepository.CreateAsync(office);

            if (model.Photo != null)
                await _blobService.UploadAsync(model.Photo, cancellationToken);

            return await GetOfficeDTOWithPhotoAsync(office);
        }

        /// <summary>
        /// Updates office with a specific id in database
        /// </summary>
        /// <param name="id">Office id</param>
        /// <param name="model">Model for updating office</param>
        public async Task UpdateAsync(Guid id, UpdateOfficeModel model, CancellationToken cancellationToken = default)
        {
            await IsBlobFileNameValid(model.Photo, cancellationToken);
            await IsModelValid(model, new UpdateOfficeValidator(), cancellationToken);

            var oldOffice = await _officeRepository.GetItemAsync(id, cancellationToken);

            if (oldOffice == null)
                throw new OfficeNotFoundException(id);

            if (oldOffice.Address.City != model.City || oldOffice.OfficeNumber != model.OfficeNumber)
                await IsOfficeNumberValid(model.City, model.OfficeNumber);

            var office = _mapper.Map<Office>(model);
            await _officeRepository.UpdateAsync(id, office);

            if (oldOffice.Photo != null && await _blobService.IsBlobExist(oldOffice.Photo.Name))
                await _blobService.DeleteAsync(oldOffice.Photo.Name);

            if (model.Photo != null)
                await _blobService.UploadAsync(model.Photo);
        }

        /// <summary>
        /// Updates status for office with a specific id
        /// </summary>
        /// <param name="id">Office id</param>
        /// <param name="newStatus">new status</param>
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

        /// <param name="pageNumber">number of page</param>
        /// <param name="pageSize">size of page</param>
        /// <returns>Information about offices</returns>
        public IEnumerable<OfficeDTO> GetOfficesPage(int pageNumber, int pageSize)
        {
            var offices = _officeRepository.GetItems();
            offices = offices.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            return _mapper.Map<IEnumerable<OfficeDTO>>(offices);
        }

        /// <param name="id">Office id</param>
        /// <returns>info about an office with a specific id</returns>
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

        private async Task IsBlobFileNameValid(IFormFile? file, CancellationToken cancellationToken = default)
        {
            if (file != null && await _blobService.IsBlobExist(file.FileName, cancellationToken))
                throw new BlobNameIsNotValidException(file.FileName);
        }

        private async Task IsOfficeNumberValid(string city, int officeNumber, CancellationToken cancellationToken = default)
        {
            var isOfficeNumberInvalid = await _officeRepository.IsItemExistAsync(x => x.Address.City == city &&
                                                                                      x.OfficeNumber == officeNumber,
                                                                                      cancellationToken);

            if (isOfficeNumberInvalid)
                throw new CityAlreadyHaveOfficeWithThatNumberException(city, officeNumber);
        }

        private async Task IsModelValid<Tmodel>(Tmodel model, IValidator<Tmodel> validator, CancellationToken cancellationToken = default)
        {
            var validationResult = await validator.ValidateAsync(model, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }
    }
}
