using XXXXMedia.Shared.Persistence.Entities;

namespace XXXXMedia.Shared.Persistence.Repositories.Interfaces
{
    public interface IClientRepository
    {
        Task<Client?> GetByIdAsync(int id);
        Task<IEnumerable<Client>> GetAllAsync();
        Task AddAsync(Client client);
        Task SaveChangesAsync();
    }
}
