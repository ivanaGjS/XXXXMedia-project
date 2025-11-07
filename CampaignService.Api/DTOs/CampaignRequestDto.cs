namespace CampaignService.Api.DTOs
{
    public class CampaignRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Recipients { get; set; } = new();
    }
}
