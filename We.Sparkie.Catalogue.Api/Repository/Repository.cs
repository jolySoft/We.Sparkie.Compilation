using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using We.Sparkie.Catalogue.Api.Entities;

namespace We.Sparkie.Catalogue.Api.Repository
{
    public class Repository<TEntity> where TEntity : Entity
    {
        private readonly CatalogueDbContext _dbContext;

        public Repository(CatalogueDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<TEntity>> Get()
        {
            return _dbContext.Set<TEntity>().ToListAsync();
        }

        public Task<TEntity> Get(Guid id)
        {
            return _dbContext.Set<TEntity>().SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task Insert(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public async Task Delete(Guid id)
        {
            var entity = await Get(id);
            if (entity == null) return;
            Delete(entity);
        }
    }
}