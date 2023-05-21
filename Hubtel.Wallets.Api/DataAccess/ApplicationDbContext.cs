using Hubtel.Wallets.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Hubtel.Wallets.Api.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Wallet> Wallets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("Users");
                b.HasMany(a => a.Wallets).WithOne(b => b.ApplicationUser).HasForeignKey(b => b.UserId);
            });

            builder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.ToTable("UserLogins");
            });

            builder.Entity<IdentityUserToken<string>>(b =>
            {
                b.ToTable("UserTokens");
            });

            builder.Entity<IdentityRole>(b =>
            {
                b.ToTable("Roles");
            });

            builder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserRole<string>>(b =>
            {
                b.ToTable("UserRoles");
            });

            builder.Entity<Wallet>(b =>
            {
                b.HasIndex(i => i.AccountNumber).IsUnique();
                b.HasIndex(i => i.Owner);
                b.Property(p => p.AccountNumber).IsRequired().HasMaxLength(20);
                b.Property(p => p.AccountScheme).IsRequired();
                b.Property(p => p.Type).IsRequired().HasMaxLength(4);
                b.Property(p => p.Name).IsRequired().HasMaxLength(100);
                b.Property(p => p.Owner).IsRequired().HasMaxLength(13);
                b.Property(p => p.CreatedAt).IsRequired();
            });
        }
    }
}
