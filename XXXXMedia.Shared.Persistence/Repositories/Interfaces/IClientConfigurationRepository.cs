using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XXXXMedia.Shared.Persistence.Entities;

namespace XXXXMedia.Shared.Persistence.Repositories.Interfaces
{
    public interface IClientConfigurationRepository
    {
        Task<ClientConfiguration?> GetByIdAsync(int id);
        Task<IEnumerable<ClientConfiguration>> GetAllAsync();
        Task AddAsync(ClientConfiguration config);
        Task SaveChangesAsync();
    }
}
