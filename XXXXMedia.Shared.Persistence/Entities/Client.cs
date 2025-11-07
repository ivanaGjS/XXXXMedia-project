using System.Collections.Generic;

namespace XXXXMedia.Shared.Persistence.Entities
{
    public class Client : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public ICollection<ClientConfiguration> Configurations { get; set; } = new List<ClientConfiguration>();
    }
}