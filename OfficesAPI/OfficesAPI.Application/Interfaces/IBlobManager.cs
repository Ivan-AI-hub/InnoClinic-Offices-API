using Microsoft.AspNetCore.Http;
using OfficesAPI.Application.Models;

namespace OfficesAPI.Application.Interfaces
{
    public interface IBlobManager
    {
        Task<Blob?> DownloadAsync(string blobFileName, CancellationToken cancellationToken = default);
        Task UploadAsync(IFormFile blob, CancellationToken cancellationToken = default);
    }
}