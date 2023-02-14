using BdTracker.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BdTracker.Groups.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new GroupConfiguration());

            modelBuilder.Entity<Group>().HasData(
                new Group
                {
                    Id = Guid.Parse("986fddc4-471d-436d-a86d-bd56add80ece"),
                    Name = "BdTracker Admins"
                }
            );
            // modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public DbSet<Group> Groups { get; set; } = default!;
    }
}