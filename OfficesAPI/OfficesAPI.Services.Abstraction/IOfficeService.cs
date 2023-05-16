using OfficesAPI.Services.Abstraction.Models;

namespace OfficesAPI.Services.Abstraction
{
    public interface IOfficeService
    {
        /// <summary>
        /// Creates the office in the system
        /// </summary>
        /// <param name="model">Model for creating office</param>
        Task<OfficeDTO> CreateAsync(CreateOfficeModel model, CancellationToken cancellationToken = default);

        /// <param name="id">Office id</param>
        /// <returns>info about an office with a specific id</returns>
        Task<OfficeDTO> GetOfficeAsync(Guid id, CancellationToken cancellationToken = default);

        /// <param name="pageNumber">number of page</param>
        /// <param name="pageSize">size of page</param>
        /// <returns>Information about offices</returns>
        IEnumerable<OfficeDTO> GetOffices(int pageNumber, int pageSize);

        /// <summary>
        /// Updates office with a specific id in database
        /// </summary>
        /// <param name="id">Office id</param>
        /// <param name="model">Model for updating office</param>
        Task UpdateAsync(Guid id, UpdateOfficeModel model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates status for office with a specific id
        /// </summary>
        /// <param name="id">Office id</param>
        /// <param name="newStatus">new status</param>
        Task UpdateStatus(Guid id, bool newStatus, CancellationToken cancellationToken = default);
    }
}