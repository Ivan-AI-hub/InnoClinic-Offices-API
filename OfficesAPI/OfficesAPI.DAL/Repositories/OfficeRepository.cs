using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OfficesAPI.Application.Interfaces;
using OfficesAPI.Domain;
using System.Linq.Expressions;

namespace OfficesAPI.DAL.Repositories
{
    public class OfficeRepository : IOfficeRepository
    {
        private IMongoCollection<Office> _officeCollection;
        public OfficeRepository(IOptions<OfficesDatabaseSettings> databaseSettings)
        {
            var settings = databaseSettings.Value;
            var client = new MongoClient(settings.ConnectionString);
            _officeCollection = client.GetDatabase(settings.DatabaseName).GetCollection<Office>(settings.OfficesCollectionName);
        }

        public Task CreateAsync(Office item) => _officeCollection.InsertOneAsync(item);

        public Task DeleteAsync(Guid id) => _officeCollection.DeleteOneAsync(x => x.Id == id);
        public Task UpdateAsync(Guid id, Office updatedOffice) => _officeCollection.ReplaceOneAsync(i => i.Id == id, updatedOffice);

        public Task<Office> GetItemAsync(Guid id) => _officeCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        public IQueryable<Office> GetItems() => _officeCollection.AsQueryable();
        public IQueryable<Office> GetItemsByCondition(Expression<Func<Office, bool>> predicate) => GetItems().Where(predicate);

        public Task<bool> IsItemExistAsync(Expression<Func<Office, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return _officeCollection.AsQueryable().AnyAsync(predicate, cancellationToken);
        }

    }
}
