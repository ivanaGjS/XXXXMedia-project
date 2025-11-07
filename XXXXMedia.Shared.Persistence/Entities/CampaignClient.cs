namespace XXXXMedia.Shared.Persistence.Entities
{
    public class CampaignClient : BaseEntity
    {
        public int CampaignId { get; set; }
        public Campaign? Campaign { get; set; }
        public bool IsActive {  get; set; }

        public int ClientConfigurationId { get; set; }
        public ClientConfiguration? ClientConfiguration { get; set; }

        public bool EmailSent { get; set; } = false;
    }
}
