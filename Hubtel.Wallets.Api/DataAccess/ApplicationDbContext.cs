﻿using EntityFramework.Exceptions.SqlServer;
using Hubtel.Wallets.Api.Constants;
using Hubtel.Wallets.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Hubtel.Wallets.Api.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Wallet> Wallets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseExceptionProcessor();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Configures Entities

            builder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("Users");
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
                b.HasIndex(p => p.AccountNumber).IsUnique();
                b.HasIndex(p => p.Owner);
                b.HasKey(p => p.Id);
                b.Property(p => p.AccountNumber).IsRequired();
                b.Property(p => p.AccountScheme).IsRequired();
                b.Property(p => p.Type).IsRequired().HasMaxLength(4);
                b.Property(p => p.Name).IsRequired().HasMaxLength(100);
                b.Property(p => p.Owner).IsRequired().HasMaxLength(13);
                b.Property(p => p.CreatedAt).IsRequired();
            });
            #endregion
            
            SeedRoles(builder);
        }

        #region Defines private methods
        private void SeedRoles (ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(new IdentityRole()
            {
                Id = CustomIdentityConstants.RoleId,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Name = "admin",
                NormalizedName = "ADMIN"
            });
        }

        #endregion
    }
}
