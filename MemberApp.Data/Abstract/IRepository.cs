using MemberApp.Model;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MemberApp.Data.Abstract
{
    public interface IRepository<T> where T : class, IAggregateRoot
    {
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> GetAll();

        Task<int> CountAsync();

        Task<T> GetSingleAsync(long id);

        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate);

        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task DeleteWhereAsync(Expression<Func<T, bool>> predicate);

        Task CommitAsync();
    }
}
