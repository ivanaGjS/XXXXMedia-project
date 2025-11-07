using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XXXXMedia.Shared.Persistence.Entities;

namespace XXXXMedia.Shared.Persistence.Repositories.Interfaces
{
    public interface ITemplateRepository
    {
        Task<Template?> GetByIdAsync(int id);
        Task<IEnumerable<Template>> GetAllAsync();
        Task AddAsync(Template template);
        Task SaveChangesAsync();
    }
}
