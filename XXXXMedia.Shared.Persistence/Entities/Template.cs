using System.Collections.Generic;

namespace XXXXMedia.Shared.Persistence.Entities
{
    public class Template : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;

        public ICollection<ClientConfiguration> Configurations { get; set; } = new List<ClientConfiguration>();
    }
}

