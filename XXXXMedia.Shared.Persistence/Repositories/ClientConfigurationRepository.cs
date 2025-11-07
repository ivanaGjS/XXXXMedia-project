using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XXXXMedia.Shared.Persistence.Entities;
using XXXXMedia.Shared.Persistence.Repositories.Interfaces;

namespace XXXXMedia.Shared.Persistence.Repositories
{
    public class ClientConfigurationRepository : IClientConfigurationRepository
    {
        private readonly SharedDbContext _context;
        public ClientConfigurationRepository(SharedDbContext context) => _context = context;

        public async Task<ClientConfiguration?> GetByIdAsync(int id) => await _context.ClientConfigurations.FindAsync(id);
        public async Task<IEnumerable<ClientConfiguration>> GetAllAsync() => await _context.ClientConfigurations.ToListAsync();
        public async Task AddAsync(ClientConfiguration config) => await _context.ClientConfigurations.AddAsync(config);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
