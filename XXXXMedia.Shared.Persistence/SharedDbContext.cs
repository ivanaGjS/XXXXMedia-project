using Microsoft.EntityFrameworkCore;
using XXXXMedia.Shared.Persistence.Entities;

namespace XXXXMedia.Shared.Persistence
{
    public class SharedDbContext : DbContext
    {
        public SharedDbContext(DbContextOptions<SharedDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<ClientConfiguration> ClientConfigurations { get; set; } = null!;
        public DbSet<Template> Templates { get; set; } = null!;
        public DbSet<Campaign> Campaigns { get; set; } = null!;
        public DbSet<CampaignClient> CampaignClients { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Users & Roles (1:N)
            modelBuilder.Entity<Role>(b =>
            {
                b.ToTable("Roles");
                b.HasKey(r => r.Id);
                b.Property(r => r.Name).IsRequired().HasMaxLength(128);
            });

            modelBuilder.Entity<User>(b =>
            {
                b.ToTable("Users");
                b.HasKey(u => u.Id);
                b.Property(u => u.Username).IsRequired().HasMaxLength(128);
                b.Property(u => u.PasswordHash).IsRequired().HasMaxLength(500);
                b.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId).OnDelete(DeleteBehavior.Restrict);
            });

            // Clients
            modelBuilder.Entity<Client>(b =>
            {
                b.ToTable("Clients");
                b.HasKey(c => c.Id);
                b.Property(c => c.Name).IsRequired().HasMaxLength(200);
                b.Property(c => c.Email).HasMaxLength(200);
            });

            // Templates
            modelBuilder.Entity<Template>(b =>
            {
                b.ToTable("Templates");
                b.HasKey(t => t.Id);
                b.Property(t => t.Name).HasMaxLength(200);
                b.Property(t => t.Subject).HasMaxLength(500);
                b.Property(t => t.Body).IsRequired();
            });

            // ClientConfiguration (links Client <-> Template)
            modelBuilder.Entity<ClientConfiguration>(b =>
            {
                b.ToTable("ClientConfigurations");
                b.HasKey(c => c.Id);
                b.HasOne(c => c.Client).WithMany(cl => cl.Configurations).HasForeignKey(c => c.ClientId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(c => c.Template).WithMany(t => t.Configurations).HasForeignKey(c => c.TemplateId).OnDelete(DeleteBehavior.SetNull);
                b.Property(c => c.Email).HasMaxLength(200);
            });

            // Campaign
            modelBuilder.Entity<Campaign>(b =>
            {
                b.ToTable("Campaigns");
                b.HasKey(c => c.Id);
                b.Property(c => c.Name).HasMaxLength(300);
                b.Property(c => c.Status).HasMaxLength(50);
            });

            // CampaignClient
            modelBuilder.Entity<CampaignClient>(b =>
            {
                b.ToTable("CampaignClients");
                b.HasKey(cc => cc.Id);
                b.HasOne(cc => cc.Campaign).WithMany(c => c.CampaignClients).HasForeignKey(cc => cc.CampaignId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(cc => cc.ClientConfiguration).WithMany().HasForeignKey(cc => cc.ClientConfigurationId).OnDelete(DeleteBehavior.Cascade);
            });

            // Indexes and defaults
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<Client>().HasIndex(c => c.Email).IsUnique(false);
        }
    }
}
