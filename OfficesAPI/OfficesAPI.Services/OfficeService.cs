using AutoMapper;
using MediatR;
using OfficesAPI.Application.Commands.Offices.Create;
using OfficesAPI.Application.Commands.Offices.Update;
using OfficesAPI.Application.Commands.Offices.UpdateStatus;
using OfficesAPI.Application.Queries.Offices.GetItem;
using OfficesAPI.Application.Queries.Offices.GetPage;
using OfficesAPI.Domain;
using OfficesAPI.Services.Models;
using OfficesAPI.Services.Results;

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

        public async Task<ServiceValueResult<OfficeDTO>> CreateAsync(CreateOfficeModel model, CancellationToken cancellationToken = default)
        {
            if (model.Photo != null)
            {
                var blobFileResult = await IsBlobFileNameValid(model.Photo.FileName);
                if (!blobFileResult.IsComplite)
                    return new ServiceValueResult<OfficeDTO>(blobFileResult);
            }

            var request = _mapper.Map<CreateOffice>(model);

            var applicationResult = await _mediator.Send(request, cancellationToken);

            if (!applicationResult.IsComplite)
                return new ServiceValueResult<OfficeDTO>(applicationResult);

            if (model.Photo != null)
            {
                await _blobService.UploadAsync(model.Photo, cancellationToken);
            }

            var office = await GetOfficeDTOWithPhotoAsync(applicationResult.Value);
            return new ServiceValueResult<OfficeDTO>(office);
        }
        public async Task<ServiceVoidResult> UpdateAsync(Guid id, UpdateOfficeModel model, CancellationToken cancellationToken = default)
        {
            if (model.Photo != null)
            {
                var blobFileResult = await IsBlobFileNameValid(model.Photo.FileName);
                if (!blobFileResult.IsComplite)
                    return new ServiceVoidResult(blobFileResult);
            }

            var request = _mapper.Map<UpdateOffice>(model);
            request.Id = id;

            var applicationResult = await _mediator.Send(request, cancellationToken);
            if (!applicationResult.IsComplite)
                return new ServiceVoidResult(applicationResult);

            if (applicationResult.OldValue.Photo != null && await _blobService.IsBlobExist(applicationResult.OldValue.Photo.Name))
            {
                await _blobService.DeleteAsync(applicationResult.OldValue.Photo.Name);
            }

            if (model.Photo != null)
            {
                await _blobService.UploadAsync(model.Photo);
            }

            return new ServiceVoidResult(applicationResult);
        }
        public async Task<ServiceVoidResult> UpdateStatus(Guid id, bool newStatus, CancellationToken cancellationToken = default)
        {
            var applicationResult = await _mediator.Send(new UpdateOfficeStatus(id, newStatus), cancellationToken);
            return new ServiceVoidResult(applicationResult);
        }

        public async Task<IEnumerable<OfficeDTO>> GetOfficesPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var offices = await _mediator.Send(new GetOfficesPage(pageNumber, pageSize), cancellationToken);
            return _mapper.Map<IEnumerable<OfficeDTO>>(offices);
        }

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


        private async Task<IServiceResult> IsBlobFileNameValid(string blobFileName)
        {
            if (await _blobService.IsBlobExist(blobFileName))
            {
                return new ServiceVoidResult(errors: "File with the same name already exist in database");
            }
            return new ServiceVoidResult();
        }
    }
}
