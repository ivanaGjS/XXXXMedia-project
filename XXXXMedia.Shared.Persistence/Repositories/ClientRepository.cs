using Microsoft.EntityFrameworkCore;
using XXXXMedia.Shared.Persistence.Entities;
using XXXXMedia.Shared.Persistence.Repositories.Interfaces;

namespace XXXXMedia.Shared.Persistence.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly SharedDbContext _context;
        public ClientRepository(SharedDbContext context) => _context = context;

        public async Task<Client?> GetByIdAsync(int id) => await _context.Clients.FindAsync(id);
        public async Task<IEnumerable<Client>> GetAllAsync() => await _context.Clients.ToListAsync();
        public async Task AddAsync(Client client) => await _context.Clients.AddAsync(client);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
