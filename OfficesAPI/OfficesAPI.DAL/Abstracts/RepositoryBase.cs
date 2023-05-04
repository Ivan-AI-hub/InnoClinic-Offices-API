using Microsoft.EntityFrameworkCore;
using OfficesAPI.Application.Interfaces;
using System.Linq.Expressions;

namespace OfficesAPI.DAL.Abstracts
{
    public abstract class RepositoryBase<T> : IRepository<T>
       where T : class
    {
        protected OfficesContext Context;
        public RepositoryBase(OfficesContext context)
        {
            Context = context;
        }

        public virtual void Create(T item)
        {
            Context.Set<T>().Add(item);
        }
        public virtual void Update(T item)
        {
            Context.Set<T>().Update(item);
        }

        public virtual void Delete(T item)
        {
            Context.Set<T>().Remove(item);
        }

        public Task<bool> IsItemExistAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return Context.Set<T>().AnyAsync(predicate, cancellationToken);
        }

        public IQueryable<T> GetItemsByCondition(Expression<Func<T, bool>> predicate, bool trackChanges)
        {
            return GetItems(trackChanges).Where(predicate);
        }

        public abstract IQueryable<T> GetItems(bool trackChanges);
    }
}
