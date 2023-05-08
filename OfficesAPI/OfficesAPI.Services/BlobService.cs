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

        /// <summary>
        /// Uploads a blob to an azure server
        /// </summary>
        public async Task UploadAsync(IFormFile blob, CancellationToken cancellationToken = default)
        {
            BlobContainerClient container = new BlobContainerClient(_blobStorageSettings.ConnectionString, _blobStorageSettings.ImagesContainerName);

            BlobClient client = container.GetBlobClient(blob.FileName);

            await using (Stream? data = blob.OpenReadStream())
            {
                await client.UploadAsync(data, cancellationToken);
            }
        }

        /// <summary>
        /// Downloads a blob with a specific name from an azure server
        /// </summary>
        /// <returns>Blob object if exist, otherwise returns null</returns>
        public async Task<Blob?> DownloadAsync(string blobFileName, CancellationToken cancellationToken = default)
        {
            BlobContainerClient container = new BlobContainerClient(_blobStorageSettings.ConnectionString, _blobStorageSettings.ImagesContainerName);

            BlobClient file = container.GetBlobClient(blobFileName);

            if (!await file.ExistsAsync(cancellationToken))
            {
                return null;
            }

            var content = await file.DownloadContentAsync(cancellationToken);
            string contentType = content.Value.Details.ContentType;

            return new Blob(blobFileName, contentType, content.Value.Content.ToArray());
        }

        /// <summary>
        /// Deletes a blob object with a specific name from an azure server
        /// </summary>
        public async Task DeleteAsync(string blobFileName, CancellationToken cancellationToken = default)
        {
            BlobContainerClient container = new BlobContainerClient(_blobStorageSettings.ConnectionString, _blobStorageSettings.ImagesContainerName);

            await container.DeleteBlobAsync(blobFileName, cancellationToken: cancellationToken);
        }

        /// <returns>True if exist and False if not</returns>
        public async Task<bool> IsBlobExist(string blobFileName, CancellationToken cancellationToken = default)
        {
            BlobContainerClient container = new BlobContainerClient(_blobStorageSettings.ConnectionString, _blobStorageSettings.ImagesContainerName);

            BlobClient file = container.GetBlobClient(blobFileName);
            return await file.ExistsAsync(cancellationToken);
        }
    }
}
