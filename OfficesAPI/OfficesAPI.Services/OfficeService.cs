using AutoMapper;
using MediatR;
using OfficesAPI.Application.Commands.Offices.Create;
using OfficesAPI.Application.Commands.Offices.UpdateStatus;
using OfficesAPI.Application.Queries.Offices.GetItem;
using OfficesAPI.Application.Queries.Offices.GetPage;
using OfficesAPI.Services.Models;

namespace OfficesAPI.Services
{
    public class OfficeService
    {
        private IMediator _mediator;
        private IMapper _mapper;
        private BlobService _blobService;

        public OfficeService(IMediator mediator,IMapper mapper, BlobService blobService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _blobService = blobService;
        }

        public async Task<ServiceVoidResult> CreateAsync(CreateOfficeModel model, CancellationToken cancellationToken = default)
        {
            var request = _mapper.Map<CreateOffice>(model);

            var result = await _mediator.Send(request, cancellationToken);

            if(result.IsComplite && model.Photo != null)
            {
                await _blobService.UploadAsync(model.Photo, cancellationToken);
            }

            return new ServiceVoidResult(result);
        }
        public async Task<ServiceVoidResult> UpdateStatus(Guid id, bool newStatus, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new UpdateOfficeStatus(id, newStatus), cancellationToken);
            return new ServiceVoidResult(result);
        }

        public async Task<IEnumerable<OfficeDTO>> GetOfficesPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var offices = await _mediator.Send(new GetOfficesPage(pageNumber, pageSize), cancellationToken);
            return _mapper.Map<IEnumerable<OfficeDTO>>(offices);
        }

        public async Task<OfficeDTO> GetOfficeAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var officeData = await _mediator.Send(new GetOffice(id), cancellationToken);
            var office = _mapper.Map<OfficeDTO>(officeData);

            if(officeData.Photo != null)
            {
                office.Photo = await _blobService.DownloadAsync(officeData.Photo.Name, cancellationToken);
            }

            return office;
        }
    }
}
