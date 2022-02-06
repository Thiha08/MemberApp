using MemberApp.Data.Abstract;
using MemberApp.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MemberApp.Data
{
    public class Repository<T> : IRepository<T> where T : EntityBase, IAggregateRoot
    {
        private MemberAppContext _dbContext;

        private DbSet<T> _entities;

        public Repository(MemberAppContext dbContext)
        {
            _dbContext = dbContext;
            _entities = _dbContext.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _entities.AsQueryable();
        }

        public async Task<int> CountAsync()
        {
            return await _entities.CountAsync();
        }

        public IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _entities;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.AsQueryable();
        }

        public async Task<T> GetSingleAsync(long id)
        {
            return await _entities.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.FirstOrDefaultAsync(predicate);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _entities;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.Where(predicate).FirstOrDefaultAsync();
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _entities.Where(predicate);
        }

        public async Task AddAsync(T entity)
        {
            EntityEntry dbEntityEntry = _dbContext.Entry<T>(entity);
            await _entities.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            EntityEntry dbEntityEntry = _dbContext.Entry<T>(entity);

            dbEntityEntry.State = EntityState.Modified;
        }

        public async Task DeleteAsync(T entity)
        {
            EntityEntry dbEntityEntry = _dbContext.Entry<T>(entity);

            dbEntityEntry.State = EntityState.Deleted;
        }

        public async Task DeleteWhereAsync(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> entities = _entities.Where(predicate);

            foreach (var entity in entities)
            {
                _dbContext.Entry<T>(entity).State = EntityState.Deleted;
            }
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
