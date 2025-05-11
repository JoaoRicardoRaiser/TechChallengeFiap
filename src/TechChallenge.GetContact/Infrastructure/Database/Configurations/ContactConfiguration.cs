using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechChallenge.GetContact.Domain.Entities;

namespace TechChallenge.GetContact.Infrastructure.Database.Configurations;

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
