using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Infrastructure.Database.Configurations;

[ExcludeFromCodeCoverage]
public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.PhoneArea)
            .WithMany()
            .HasForeignKey(x => x.PhoneAreaCode);
    }
}
