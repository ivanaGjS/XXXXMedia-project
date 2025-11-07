using CampaignService.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using XXXXMedia.Shared.Persistence;

namespace CampaignService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CampaignsController : ControllerBase
    {
        private readonly Services.CampaignService _campaignService;
        private readonly SharedDbContext _dbContext;

        public CampaignsController(Services.CampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var campaigns = await _campaignService.GetAllAsync();
            return Ok(campaigns);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var campaign = await _campaignService.GetByIdAsync(id);
            if (campaign == null)
                return NotFound();

            return Ok(campaign);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCampaign([FromBody] CampaignRequestDto request)
        {

            var campaign = await _campaignService.CreateCampaignAsync(request.Name, request.Recipients);
           
            return Ok(new { campaignId = campaign.Id });
        }
    }
}
