using System.Linq.Expressions;

namespace OfficesAPI.Domain.Interfaces
{
    public interface IRepository<T>
    {

        /// <param name="id">item's id</param>
        /// <returns>Item if it wes found or null if not</returns>
        Task<T?> GetItemAsync(Guid id, CancellationToken cancellationToken = default);

        /// <param name="predicate">Special predicate for element search</param>
        /// <returns>IQueryable collection</returns>
        public IQueryable<T> GetItemsByCondition(Expression<Func<T, bool>> predicate);

        /// <returns>queryable items from the database</returns>
        public IQueryable<T> GetItems();

        /// <summary>
        /// Create item in database
        /// </summary>
        public Task CreateAsync(T item);

        /// <summary>
        /// Update item in database
        /// </summary>
        public Task UpdateAsync(Guid id, T updatedItem);

        /// <summary>
        /// Delete item from database
        /// </summary>
        public Task DeleteAsync(Guid id);

        /// <returns>true if the element exists, and false if not</returns>
        public Task<bool> IsItemExistAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    }
}
