﻿using Microsoft.EntityFrameworkCore;
using MultiTenantAndRolesTest.Models;
using System.Reflection.Emit;

namespace MultiTenantAndRolesTest.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<UserBlacklist> UserBlacklist { get; set; }
        public DbSet<JwtBlacklist> JwtBlacklist { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //List<Role> roles = new List<Role>()
            //{
            //    new Role
            //    {
            //        Name="Admin",
            //    },
            //    new Role
            //    {
            //        Name="User",
            //    },
            //};
            //builder.Entity<Role>().HasData(roles);

            base.OnModelCreating(builder);

            builder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);

                entity.HasIndex(e => e.Email).IsUnique();
            });

            builder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

                entity.HasIndex(e => e.Name).IsUnique();
            });

            builder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Role)
                    .WithMany()
                    .HasForeignKey(e => e.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.UserId, e.RoleId }).IsUnique();
            });

            builder.Entity<Permission>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

                entity.HasIndex(e => e.Name).IsUnique();
            });

            builder.Entity<RolePermission>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Role)
                    .WithMany()
                    .HasForeignKey(e => e.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Permission)
                    .WithMany()
                    .HasForeignKey(e => e.PermissionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.RoleId, e.PermissionId }).IsUnique();
            });
            builder.Entity<UserBlacklist>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();

                entity.HasIndex(e => e.UserId).IsUnique();
            });
            builder.Entity<JwtBlacklist>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.token).IsRequired();
                entity.Property(e => e.expires).IsRequired();
            });

        }
    }
}