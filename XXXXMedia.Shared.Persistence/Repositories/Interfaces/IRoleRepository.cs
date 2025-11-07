using XXXXMedia.Shared.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XXXXMedia.Shared.Persistence.Repositories.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(Guid id);
    Task<Role?> GetByNameAsync(string name);
    Task<IEnumerable<Role>> GetAllAsync();
    Task AddAsync(Role role);
    Task UpdateAsync(Role role);
    Task DeleteAsync(Guid id);
    Task SaveChangesAsync();
}
