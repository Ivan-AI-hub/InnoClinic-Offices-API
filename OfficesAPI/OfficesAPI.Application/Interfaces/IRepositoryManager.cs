namespace OfficesAPI.Application.Interfaces
{
    public interface IRepositoryManager
    {
        IOfficeRepository OfficeRepository { get; }
        /// <summary>
        /// Saves all changes made in repositories.
        /// </summary>
        public Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
