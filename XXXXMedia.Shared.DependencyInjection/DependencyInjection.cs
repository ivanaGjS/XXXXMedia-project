using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XXXXMedia.Shared.Persistence;
using XXXXMedia.Shared.Persistence.Repositories;
using XXXXMedia.Shared.Persistence.Repositories.Interfaces;

namespace XXXXMedia.Shared.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSharedDependencies(this IServiceCollection services, IConfiguration? configuration = null)
        {
            // Add DbContext if configuration provided
            if (configuration != null)
            {
                services.AddDbContext<SharedDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            }

            // Register Repositories
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<ITemplateRepository, TemplateRepository>();
            services.AddScoped<ICampaignRepository, CampaignRepository>();
            services.AddScoped<IClientConfigurationRepository, ClientConfigurationRepository>();

            return services;
        }
    }
}
