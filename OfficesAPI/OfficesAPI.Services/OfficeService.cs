using AutoMapper;
using MediatR;
using OfficesAPI.Application.Commands.Offices.Create;
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

        public async Task<ServiceVoidResult> Create(CreateOfficeModel model, CancellationToken cancellationToken = default)
        {
            var request = _mapper.Map<CreateOffice>(model);

            var result = await _mediator.Send(request, cancellationToken);

            if(result.IsComplite && model.Photo != null)
            {
                await _blobService.UploadAsync(model.Photo, cancellationToken);
            }

            return new ServiceVoidResult(result);
        }

        public async Task<IEnumerable<OfficeDTO>> GetOfficesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var offices = await _mediator.Send(new GetOfficesPage(pageNumber, pageSize), cancellationToken);
            return _mapper.Map<IEnumerable<OfficeDTO>>(offices);
        }
    }
}
