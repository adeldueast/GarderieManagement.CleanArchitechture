﻿using GarderieManagementClean.Domain.Entities;
using GarderieManagementClean.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GarderieManagementClean.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Garderie> Garderies { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Enfant> Enfants { get; set; }

        public DbSet<Photo> Photos { get; set; }


        public DbSet<JournalDeBord> JournalDeBords { get; set; }

        public DbSet<Attendance> Attendances { get; set; }


        public DbSet<RefreshToken> RefreshTokens { get; set; }


        public DbSet<TutorEnfant> TutorEnfant { get; set; }

        public DbSet<Notification> Notifications { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "bdfb0164-d8ee-46e6-8eab-c0a3d7a1fa2b",
                    Name = "owner",
                    ConcurrencyStamp = "1",
                    NormalizedName = "OWNER"
                },
                new IdentityRole
                {
                    Id = "8175996e-0eb2-4d60-9cb9-9f1575cedffc",
                    Name = "admin",
                    ConcurrencyStamp = "2",
                    NormalizedName = "ADMIN"
                }, new IdentityRole
                {
                    Id = "00af114b-a523-4718-bdff-e539404775db",
                    Name = "employee",
                    ConcurrencyStamp = "3",
                    NormalizedName = "EMPLOYEE"
                }, new IdentityRole
                {
                    Id = "ef771148-39c6-4fc6-8aeb-59800edc3cac",
                    Name = "tutor",
                    ConcurrencyStamp = "4",
                    NormalizedName = "TUTOR"
                }
              );


            modelBuilder.Entity<TutorEnfant>()
                .HasKey(te => new { te.EnfantId, te.ApplicationUserId });

            modelBuilder.Entity<Photo>()
                .HasMany(p => p.Enfants)
                .WithMany(e => e.Photos);


            modelBuilder.Entity<Photo>()
               .HasOne(p => p.PhotoCouvertureDe)
               .WithOne(e => e.PhotoCouverture)
               .HasForeignKey<Photo>(p => p.EnfantId)
               .OnDelete(DeleteBehavior.SetNull);

            //https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-6.0#add-all-user-navigation-properties
            modelBuilder.Entity<ApplicationUser>(b =>
            {

                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne()
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne()
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne()
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne()
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
        }
    }
}
