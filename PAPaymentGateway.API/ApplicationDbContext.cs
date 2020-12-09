using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PAPaymentGateway.Core.Models;

namespace PAPaymentGateway
{
    /// <summary>
    /// The main application DB Context that is used for the API.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Merchant> Merchants { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            // Override default Identity Names.
            builder.Entity<IdentityUser>().ToTable("Users","dbo");
            builder.Entity<IdentityRole>().ToTable("Roles", "dbo");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "dbo");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "dbo");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "dbo");
            builder.Entity<IdentityRoleClaim<string>> ().ToTable("RoleClaims", "dbo");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "dbo");
        }
    }
}
