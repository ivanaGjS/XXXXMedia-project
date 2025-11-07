using Microsoft.EntityFrameworkCore;
using XXXXMedia.Shared.Persistence.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XXXXMedia.Shared.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly SharedDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(SharedDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
    public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);
    public async Task<T> AddAsync(T entity){ await _dbSet.AddAsync(entity); await _context.SaveChangesAsync(); return entity; }
    public async Task<T> UpdateAsync(T entity){ _dbSet.Update(entity); await _context.SaveChangesAsync(); return entity; }
    public async Task DeleteAsync(Guid id){ var ent = await GetByIdAsync(id); if(ent!=null){ _dbSet.Remove(ent); await _context.SaveChangesAsync(); } }
}
