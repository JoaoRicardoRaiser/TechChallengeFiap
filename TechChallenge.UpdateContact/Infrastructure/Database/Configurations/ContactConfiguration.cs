using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechChallenge.CreateContact.Domain.Entities;

namespace TechChallenge.CreateContact.Infrastructure.Database.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable(nameof(Contact));

        builder.HasOne(c => c.PhoneArea)
            .WithMany()
            .HasForeignKey(c => c.PhoneAreaCode);
    }
}
