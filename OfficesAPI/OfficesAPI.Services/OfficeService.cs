using MediatR;
using OfficesAPI.Application.Commands.Offices.Create;
using OfficesAPI.Services.Models;

namespace OfficesAPI.Services
{
    public class OfficeService
    {
        private IMediator _mediator;
        private BlobService _blobService;

        public OfficeService(IMediator mediator, BlobService blobService)
        {
            _mediator = mediator;
            _blobService = blobService;
        }

        public async Task<ServiceVoidResult> Create(CreateOfficeModel model, CancellationToken cancellationToken = default)
        {
            var request = new CreateOffice(model.Photo?.FileName,
                                           model.City,
                                           model.Street,
                                           model.HouseNumber,
                                           model.OfficeNumber,
                                           model.PhoneNumber,
                                           model.Status);

            var result = await _mediator.Send(request, cancellationToken);

            if(result.IsComplite && model.Photo != null)
            {
                await _blobService.UploadAsync(model.Photo, cancellationToken);
            }

            return new ServiceVoidResult(result);
        }
    }
}
