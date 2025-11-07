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
    public class TemplateRepository : ITemplateRepository
    {
        private readonly SharedDbContext _context;
        public TemplateRepository(SharedDbContext context) => _context = context;

        public async Task<Template?> GetByIdAsync(int id) => await _context.Templates.FindAsync(id);
        public async Task<IEnumerable<Template>> GetAllAsync() => await _context.Templates.ToListAsync();
        public async Task AddAsync(Template template) => await _context.Templates.AddAsync(template);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
