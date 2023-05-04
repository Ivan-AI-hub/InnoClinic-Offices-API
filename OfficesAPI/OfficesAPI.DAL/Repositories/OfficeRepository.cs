using Microsoft.EntityFrameworkCore;
using OfficesAPI.Application.Interfaces;
using OfficesAPI.DAL.Abstracts;
using OfficesAPI.Domain;

namespace OfficesAPI.DAL.Repositories
{
    public class OfficeRepository : RepositoryBase<Office>, IOfficeRepository
    {
        public OfficeRepository(OfficesContext context) : base(context)
        {
        }

        public override IQueryable<Office> GetItems(bool trackChanges)
        {
            var items = Context.Offices;
            return trackChanges ? items : items.AsNoTracking();
        }
    }
}
