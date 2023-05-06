using System.Linq.Expressions;

namespace OfficesAPI.Application.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> GetItemAsync(Guid id);

        /// <param name="predicate">Special predicate for element search</param>
        /// <returns>The element, if it was found in the database or null</returns>
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
