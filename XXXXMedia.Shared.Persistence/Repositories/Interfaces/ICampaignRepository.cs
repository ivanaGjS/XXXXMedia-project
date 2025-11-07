using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XXXXMedia.Shared.Persistence.Entities;

namespace XXXXMedia.Shared.Persistence.Repositories.Interfaces
{
    public interface ICampaignRepository
    {
        Task<Campaign?> GetByIdAsync(int id);
        Task<IEnumerable<Campaign>> GetAllAsync();
        Task AddAsync(Campaign campaign);
        Task SaveChangesAsync();
    }
}
