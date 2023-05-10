using Microsoft.AspNetCore.Http;
using OfficesAPI.Services.Abstraction.Models;

namespace OfficesAPI.Services.Abstraction
{
    public interface IBlobService
    {

        /// <summary>
        /// Deletes a blob object with a specific name from an azure server
        /// </summary>
        Task DeleteAsync(string blobFileName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads a blob with a specific name from an azure server
        /// </summary>
        /// <returns>Blob object if exist, otherwise returns null</returns>
        Task<Blob?> DownloadAsync(string blobFileName, CancellationToken cancellationToken = default);

        /// <returns>True if exist and False if not</returns>
        Task<bool> IsBlobExist(string blobFileName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads a blob to an azure server
        /// </summary>
        Task UploadAsync(IFormFile blob, CancellationToken cancellationToken = default);
    }
}