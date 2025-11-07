# XXXXMedia-project
Project for sending emails to clients

First let me just say that this is not a finished project. It is a work in progress because there are lot of things missing.

Used Onion Clean Architecture with Microservices as EmailWorker, TemplateService, CampaignService so they can can scale independently based on its workload.
Implement the event-driven approach with Azure Service Bus. 
The user and client can login to this system and authenticate themselves by the role using JWT and also generatae tokens.
Used SendGrid as EmailService because is built for high volume (100k+ emails).Handles retries, queues, and anti-spam rules. Provides analytics, tracking opens/clicks, and bounces.And can send templated emails.


CampaignService.Api

Publishing messages (events) to a message bus (Azure Service Bus or a queue) so 
that the EmailWorker can send the emails asynchronously.

EmailWorkerService

Listens to the Service Bus.
Gets data (template + recipient info) from SharedDb.
Sends emails through SendGrid.


Admin UI triggers POST /api/campaigns/create with a campaign name.
System creates a Campaign and links all active clients.
CampaignService.Api sends CampaignId message to email-queue.
EmailWorkerService receives it → loads campaign → loops through all clients → gets each template → sends email via SendGrid.
Marks emails as sent in CampaignClients.

The first thing missing is the database, so for this project I haven't create any database and used any migrations.
The basic CRUD operations are also missing due to highlight the sending email process.



