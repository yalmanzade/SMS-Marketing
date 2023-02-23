using E_VilleMarketing.Models;
using Microsoft.EntityFrameworkCore;

namespace E_VilleMarketing.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
        public DbSet<Account> Logins { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Facebook> FacebookAccounts { get; set; }
        public DbSet<TikTok> tikTokAccounts { get; set; }
        public DbSet<Twitter> TwitterAccounts { get; set; }
        public DbSet<Twilio> TwilioAccounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UsageTracker> BusinessUsageTracker { get; set; }
    }
}
