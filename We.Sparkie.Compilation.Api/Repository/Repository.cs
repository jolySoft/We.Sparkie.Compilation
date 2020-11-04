using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using We.Sparkie.Compilation.Api.Entities;

namespace We.Sparkie.Compilation.Api.Repository
{
    public class Repository<TEntity> : IDisposable where TEntity : Entity
    {
        private readonly CompilationDbContext _dbContext;

        public Repository(CompilationDbContext dbContext)
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

        public Task Insert(TEntity entity)
        {
            return _dbContext.Set<TEntity>().AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public async Task Delete(Guid id)
        {
            var entity = await Get(id);
            if (entity == null) return;
            Delete(entity);
        }

        private void ReleaseUnmanagedResources()
        {
            _dbContext.SaveChanges();
        }

        private void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
                _dbContext?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Repository()
        {
            Dispose(false);
        }
    }
}