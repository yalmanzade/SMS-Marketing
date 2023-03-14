using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SMS_Marketing.Models;

namespace SMS_Marketing.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Authorization> Authorization { get; set; } = default!;
        public DbSet<TwitterAuth> TwitterAuth { get; set; } = default!;
        public DbSet<FacebookAuth> FacebookAuth { get; set; } = default!;
        public DbSet<TwilioAuth> TwilioAuth { get; set; } = default!;
        public DbSet<AppSettings> AppSettings { get; set; } = default!;
        public DbSet<Organization> Organizations { get; set; } = default!;
        public DbSet<Group> Groups { get; set; } = default!;
        public DbSet<Customer> Customers { get; set; } = default!;
        public DbSet<Invite> Invites { get; set; } = default!;
    }
}