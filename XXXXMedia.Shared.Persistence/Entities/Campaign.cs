using System.Collections.Generic;

namespace XXXXMedia.Shared.Persistence.Entities
{
    public class Campaign : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public string Recipients { get; set; } = string.Empty;

        public ICollection<CampaignClient> CampaignClients { get; set; } = new List<CampaignClient>();
    }
}
