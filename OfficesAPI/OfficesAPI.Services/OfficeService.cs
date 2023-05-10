using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using OfficesAPI.Application.Commands.Offices.Create;
using OfficesAPI.Application.Commands.Offices.Update;
using OfficesAPI.Application.Commands.Offices.UpdateStatus;
using OfficesAPI.Application.Queries.Offices.GetItem;
using OfficesAPI.Application.Queries.Offices.GetPage;
using OfficesAPI.Domain;
using OfficesAPI.Services.Models;


namespace OfficesAPI.Services
{
    public class OfficeService
    {
        private IMediator _mediator;
        private IMapper _mapper;
        private BlobService _blobService;

        public OfficeService(IMediator mediator, IMapper mapper, BlobService blobService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _blobService = blobService;
        }

        /// <summary>
        /// Creates the officeData in the system
        /// </summary>
        /// <param name="model">Model for creating officeData</param>
        public async Task<OfficeDTO> CreateAsync(CreateOfficeModel model, CancellationToken cancellationToken = default)
        {
            await IsBlobFileNameValid(model.Photo);

            var request = _mapper.Map<CreateOffice>(model);

            var officeData = await _mediator.Send(request, cancellationToken);

            if (model.Photo != null)
            {
                await _blobService.UploadAsync(model.Photo, cancellationToken);
            }

            var office = await GetOfficeDTOWithPhotoAsync(officeData);
            return office;
        }

        /// <summary>
        /// Updates officeData with a specific id in database
        /// </summary>
        /// <param name="id">Office id</param>
        /// <param name="model">Model for updating officeData</param>
        public async Task UpdateAsync(Guid id, UpdateOfficeModel model, CancellationToken cancellationToken = default)
        {
            await IsBlobFileNameValid(model.Photo);    

            var request = _mapper.Map<UpdateOffice>(model);
            request.Id = id;

            var oldOffice = await _mediator.Send(request, cancellationToken);

            if (oldOffice.Photo != null && await _blobService.IsBlobExist(oldOffice.Photo.Name))
            {
                await _blobService.DeleteAsync(oldOffice.Photo.Name);
            }

            if (model.Photo != null)
            {
                await _blobService.UploadAsync(model.Photo);
            }
        }

        /// <summary>
        /// Updates status for officeData with a specific id
        /// </summary>
        /// <param name="id">Office id</param>
        /// <param name="newStatus">new status</param>
        public async Task UpdateStatus(Guid id, bool newStatus, CancellationToken cancellationToken = default)
        {
             await _mediator.Send(new UpdateOfficeStatus(id, newStatus), cancellationToken);
        }

        /// <param name="pageNumber">number of page</param>
        /// <param name="pageSize">size of page</param>
        /// <returns>Information about offices</returns>
        public async Task<IEnumerable<OfficeDTO>> GetOfficesPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var offices = await _mediator.Send(new GetOfficesPage(pageNumber, pageSize), cancellationToken);
            return _mapper.Map<IEnumerable<OfficeDTO>>(offices);
        }

        /// <param name="id">Office id</param>
        /// <returns>info about an officeData with a specific id</returns>
        public async Task<OfficeDTO> GetOfficeAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var officeData = await _mediator.Send(new GetOffice(id), cancellationToken);
            return await GetOfficeDTOWithPhotoAsync(officeData);
        }

        private async Task<OfficeDTO> GetOfficeDTOWithPhotoAsync(Office officeData, CancellationToken cancellationToken = default)
        {
            var office = _mapper.Map<OfficeDTO>(officeData);

            if (officeData.Photo != null)
            {
                office.Photo = await _blobService.DownloadAsync(officeData.Photo.Name, cancellationToken);
            }

            return office;
        }

        private async Task IsBlobFileNameValid(IFormFile? file)
        {
            if (file != null && await _blobService.IsBlobExist(file.FileName))
                throw new BlobNameIsNotValidException(file.FileName);
        }
    }
}
