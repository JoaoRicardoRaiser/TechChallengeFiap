using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechChallenge.UpdateContact.Domain.Entities;

namespace TechChallenge.UpdateContact.Infrastructure.Database.Configurations;

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
