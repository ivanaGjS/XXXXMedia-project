using Azure.Core;
using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using XXXXMedia.Shared.Persistence;
using XXXXMedia.Shared.Persistence.Entities;

namespace CampaignService.Api.Services
{
    public class CampaignService
    {
        private readonly SharedDbContext _context;
        private readonly ServiceBusClient _serviceBusClient;
        private readonly IConfiguration _config;

        public CampaignService(SharedDbContext context, ServiceBusClient serviceBusClient, IConfiguration config)
        {
            _context = context;
            _serviceBusClient = serviceBusClient;
            _config = config;
        }

        public async Task<Campaign> CreateCampaignAsync(string name, List<string> recipients)
        {
            var campaign = new Campaign
            {
                Name = name,
                CreatedAt = System.DateTime.UtcNow,
                Status = "Pending"
            };

            _context.Campaigns.Add(campaign);
            await _context.SaveChangesAsync();

            // Link active clients to this campaign
            var activeClients = await _context.ClientConfigurations
                .Select(c => new CampaignClient
                {
                    CampaignId = campaign.Id,
                    ClientConfigurationId = c.Id,
                    EmailSent = false,
                    IsActive=true
                }).ToListAsync();

            _context.CampaignClients.AddRange(activeClients);
            await _context.SaveChangesAsync();

            // Publish to Service Bus
            await PublishCampaignToQueueAsync(campaign);

            return campaign;
        }

        private async Task PublishCampaignToQueueAsync(Campaign campaign)
        {
            var queueName = _config["ServiceBus:QueueName"] ?? "email-queue";
            var sender = _serviceBusClient.CreateSender(queueName);

            var payload = new
            {
                campaign.Id
            };

            var messageBody = JsonSerializer.Serialize(payload);
            var message = new ServiceBusMessage(messageBody);

            await sender.SendMessageAsync(message);
        }

        public async Task<List<Campaign>> GetAllAsync()
        {
            return await _context.Campaigns.OrderByDescending(c => c.CreatedAt).ToListAsync();
        }

        public async Task<Campaign?> GetByIdAsync(int id)
        {
            return await _context.Campaigns.FindAsync(id);
        }
    }
}