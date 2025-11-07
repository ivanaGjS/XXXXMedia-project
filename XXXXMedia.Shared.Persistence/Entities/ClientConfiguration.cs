namespace XXXXMedia.Shared.Persistence.Entities
{
    public class ClientConfiguration : BaseEntity
    {
        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public int TemplateId { get; set; }
        public Template? Template { get; set; }

        public string Email { get; set; } = string.Empty;
        public bool IsSubscribed { get; set; } = true;
    }
}
