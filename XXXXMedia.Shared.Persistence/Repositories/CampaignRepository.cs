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
    public class CampaignRepository : ICampaignRepository
    {
        private readonly SharedDbContext _context;
        public CampaignRepository(SharedDbContext context) => _context = context;

        public async Task<Campaign?> GetByIdAsync(int id) => await _context.Campaigns.FindAsync(id);
        public async Task<IEnumerable<Campaign>> GetAllAsync() => await _context.Campaigns.ToListAsync();
        public async Task AddAsync(Campaign campaign) => await _context.Campaigns.AddAsync(campaign);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
