using Azure.Messaging.ServiceBus;
using EmailWorkerService.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using XXXXMedia.Shared.Persistence;

namespace EmailWorkerService;

    public class EmailWorker : BackgroundService
    {
        private readonly ILogger<EmailWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly ServiceBusClient _serviceBusClient;
        private readonly string _queueName;

        public EmailWorker(
            ILogger<EmailWorker> logger,
            IServiceProvider serviceProvider,
            IConfiguration config,
            ServiceBusClient serviceBusClient)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _serviceBusClient = serviceBusClient;
            _queueName = config["ServiceBus:QueueName"] ?? "email-queue";
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("EmailWorker started listening on queue {queue}", _queueName);

            var processor = _serviceBusClient.CreateProcessor(_queueName, new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false
            });

            processor.ProcessMessageAsync += ProcessMessageHandler;
            processor.ProcessErrorAsync += ErrorHandler;

            await processor.StartProcessingAsync(stoppingToken);

            // Keep the worker alive
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private async Task ProcessMessageHandler(ProcessMessageEventArgs args)
        {
            try
            {
                string body = args.Message.Body.ToString();
                _logger.LogInformation("Received message: {body}", body);

                var message = JsonSerializer.Deserialize<CampaignMessage>(body);
                if (message == null)
                {
                    _logger.LogWarning("Message could not be deserialized.");
                    await args.CompleteMessageAsync(args.Message);
                    return;
                }

                // Process campaign
                await HandleCampaignAsync(message.CampaignId);

                await args.CompleteMessageAsync(args.Message);
                _logger.LogInformation("Message processed successfully for Campaign {id}", message.CampaignId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message");
            }
        }

        private async Task HandleCampaignAsync(int campaignId)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<SharedDbContext>();
            var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

            var campaign = await db.Campaigns
                .Include(c => c.CampaignClients)
                    .ThenInclude(cc => cc.ClientConfiguration)
                        .ThenInclude(cfg => cfg.Template)
                .FirstOrDefaultAsync(c => c.Id == campaignId);

            if (campaign == null)
            {
                _logger.LogWarning("Campaign {id} not found", campaignId);
                return;
            }

            foreach (var cc in campaign.CampaignClients)
            {
                var config = cc.ClientConfiguration;
                var template = config?.Template;

                if (config == null || template == null)
                {
                    _logger.LogWarning("Invalid configuration for CampaignClient {id}", cc.Id);
                    continue;
                }

                if (!config.IsSubscribed)
                {
                    _logger.LogInformation("Client {email} is unsubscribed.", config.Email);
                    continue;
                }

                try
                {
                    await emailSender.SendEmailAsync(
                        config.Email,
                        template.Subject,
                        template.Body
                    );

                    cc.EmailSent = true;
                    _logger.LogInformation("Email sent to {email}", config.Email);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send email to {email}", config.Email);
                }
            }

            await db.SaveChangesAsync();
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, "ServiceBus processing error");
            return Task.CompletedTask;
        }
    }

    public record CampaignMessage(int CampaignId);