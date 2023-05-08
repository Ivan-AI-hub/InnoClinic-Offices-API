using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OfficesAPI.Services.Models;
using OfficesAPI.Services.Settings;

namespace OfficesAPI.Services
{
    public class BlobService
    {
        private BlobStorageSettings _blobStorageSettings;
        public BlobService(IOptions<BlobStorageSettings> blobStorageSettings)
        {
            _blobStorageSettings = blobStorageSettings.Value;
            BlobContainerClient container = new BlobContainerClient(_blobStorageSettings.ConnectionString, _blobStorageSettings.ImagesContainerName);
            if (!container.Exists().Value)
                container.CreateAsync();
        }

        public async Task UploadAsync(IFormFile blob, CancellationToken cancellationToken = default)
        {
            BlobContainerClient container = new BlobContainerClient(_blobStorageSettings.ConnectionString, _blobStorageSettings.ImagesContainerName);

            BlobClient client = container.GetBlobClient(blob.FileName);

            await using (Stream? data = blob.OpenReadStream())
            {
                await client.UploadAsync(data, cancellationToken);
            }
        }

        public async Task<Blob?> DownloadAsync(string blobFileName, CancellationToken cancellationToken = default)
        {
            BlobContainerClient container = new BlobContainerClient(_blobStorageSettings.ConnectionString, _blobStorageSettings.ImagesContainerName);

            BlobClient file = container.GetBlobClient(blobFileName);

            if (!await file.ExistsAsync(cancellationToken))
            {
                return null;
            }

            //Пофиксить 
            var data = await file.OpenReadAsync(cancellationToken: cancellationToken);
            var content = await file.DownloadContentAsync(cancellationToken);
            string contentType = content.Value.Details.ContentType;

            return new Blob(blobFileName, contentType, data);
        }
    }
}
