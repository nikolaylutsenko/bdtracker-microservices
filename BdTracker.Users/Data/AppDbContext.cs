﻿using BdTracker.Shared.Entities;
using BdTracker.Users.Data;
using BdTracker.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BdTracker.Back.Data
{
    public class AppDbContext : DbContext
    {
        static AppDbContext()
        {
            // this code mark as deprecated but without it it wont create postgres enum
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Sex>();
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum<Sex>();

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = Guid.Parse("41131323-0899-4248-8a52-7b570af549d4"),
                Name = "Super",
                Surname = "Admin",
                Sex = Sex.Male,
                Birthday = new DateOnly(1987, 11, 17),
                Occupation = ".NET Developer",
                AboutMe = "Passionate programmer",
                GroupsIds = new List<string> { "986fddc4-471d-436d-a86d-bd56add80ece" }, // TODO: add this group to GroupService as Super Admin group
                WishlistId = Guid.Parse("ac8b9e86-218f-48ff-91fd-cff13101eae2")
            });
            // modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public DbSet<User> Users { get; set; } = default!;
    }
}