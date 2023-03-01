using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenIddict.EntityFrameworkCore.Models;

namespace BdTracker.Auth.Data;

public class AppDbContext : IdentityDbContext
{
    private readonly IServiceProvider _service;

    public AppDbContext(DbContextOptions<AppDbContext> options, IServiceProvider service) : base(options)
    {
        _service = service;
        this.Database.EnsureCreated();

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = "58f3dea3-67eb-4284-b4bd-e4504d8e523e",
            Name = "SuperAdmin",
            NormalizedName = "SuperAdmin".ToUpper(),
            ConcurrencyStamp = "58f3dea3-67eb-4284-b4bd-e4504d8e523e"
        });

        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = "0a26e36f-1626-4298-9a97-34a8c4118e08",
            Name = "Admin",
            NormalizedName = "Admin".ToUpper(),
            ConcurrencyStamp = "0a26e36f-1626-4298-9a97-34a8c4118e08"
        });

        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = "dd62e685-29cf-4dd7-b59b-e44022d88d29",
            Name = "User",
            NormalizedName = "User".ToUpper(),
            ConcurrencyStamp = "dd62e685-29cf-4dd7-b59b-e44022d88d29"
        });

        modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
        {
            Id = "25d733fa-b5ce-41fe-a868-beea7723a3e5",
            UserName = "SuperAdmin",
            NormalizedUserName = "SuperAdmin".ToUpper(),
            Email = "super.admin@bdtracker.com",
            EmailConfirmed = true,
            ConcurrencyStamp = "25d733fa-b5ce-41fe-a868-beea7723a3e5",
            NormalizedEmail = "super.admin@bdtracker.com".ToUpper(),
            PasswordHash = "AQAAAAEAACcQAAAAEO1PQ5JXbydpkx3TOGqWM8uW1z3dMbYFjhRqDXzTAQXzxdFyF5f6VDdMmnJk8z44TA==",
            SecurityStamp = "25d733fa-b5ce-41fe-a868-beea7723a3e5",
        });

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = "58f3dea3-67eb-4284-b4bd-e4504d8e523e",
            UserId = "25d733fa-b5ce-41fe-a868-beea7723a3e5"
        });
    }
}
