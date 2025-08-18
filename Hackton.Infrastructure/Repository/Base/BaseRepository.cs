﻿using Hackton.Domain.Base.Entity;
using Hackton.Domain.Interfaces.Base.Repository;
using Hackton.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Hackton.Infrastructure.Repository.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly HacktonContext _context;
        protected DbSet<T> _dbSet { get; set; }
        public BaseRepository(HacktonContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var entity = await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.Active).ConfigureAwait(false);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
