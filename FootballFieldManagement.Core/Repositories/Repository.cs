using FootballFieldManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballFieldManagement.Core.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : DomainObject
    {
        private DbContext _dbContext;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var entry = await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entry.Entity;
        }

        public Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            _dbContext.SaveChangesAsync();
            return _dbContext.Set<TEntity>().AddRangeAsync(entities);
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return _dbContext.Set<TEntity>();
        }

        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            var entry = _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
            return entry.Entity;
        }

        public Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            _dbContext.Set<TEntity>().RemoveRange(entities);
            _dbContext.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var entry = _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entry.Entity;
        }

        public Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            _dbContext.Set<TEntity>().UpdateRange(entities);
            _dbContext.SaveChangesAsync();
            return Task.CompletedTask;
        }
    }
}
