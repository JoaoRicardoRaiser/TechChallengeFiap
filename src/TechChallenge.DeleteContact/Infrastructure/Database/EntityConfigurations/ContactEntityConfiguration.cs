using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechChallenge.DeleteContact.Domain.Entities;

namespace TechChallenge.DeleteContact.Infrastructure.Database.EntityConfigurations;

public class ContactEntityConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
