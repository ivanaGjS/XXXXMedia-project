using XXXXMedia.Shared.Persistence.Entities;
using XXXXMedia.Shared.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XXXXMedia.Shared.Persistence.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly SharedDbContext _context;
    public RoleRepository(SharedDbContext context) { _context = context; }

    public async Task<Role?> GetByIdAsync(Guid id) => await _context.Roles.FindAsync(id);
    public async Task<Role?> GetByNameAsync(string name) => await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
    public async Task<IEnumerable<Role>> GetAllAsync() => await _context.Roles.AsNoTracking().ToListAsync();
    public async Task AddAsync(Role role) => await _context.Roles.AddAsync(role);
    public async Task UpdateAsync(Role role) => _context.Roles.Update(role);
    public async Task DeleteAsync(Guid id){ var r=await _context.Roles.FindAsync(id); if(r!=null) _context.Roles.Remove(r); }
    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
