using BdTracker.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BdTracker.Users.Data;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Surname)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Sex)
            .IsRequired();

        builder.Property(x => x.Birthday)
            .IsRequired();

        builder.Property(x => x.Occupation)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(x => x.AboutMe)
            .HasMaxLength(1000)
            .IsRequired(false);
    }
}