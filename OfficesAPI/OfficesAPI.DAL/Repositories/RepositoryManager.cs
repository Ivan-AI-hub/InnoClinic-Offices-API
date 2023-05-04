using Microsoft.Extensions.DependencyInjection;
using OfficesAPI.Application.Interfaces;

namespace OfficesAPI.DAL.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private OfficesContext _context;
        private IServiceProvider _serviceProvider;

        private IOfficeRepository? _officeRepository;
        public RepositoryManager(OfficesContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }
        public IOfficeRepository OfficeRepository
        {
            get
            {
                if (_officeRepository == null)
                    _officeRepository = _serviceProvider.GetRequiredService<IOfficeRepository>();
                return _officeRepository;
            }
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
