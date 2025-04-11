using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechChallenge.CreateContact.Domain.Entities;

namespace TechChallenge.CreateContact.Infrastructure.Database.Configurations;

public class PhoneAreaConfiguration : IEntityTypeConfiguration<PhoneArea>
{
    public void Configure(EntityTypeBuilder<PhoneArea> builder)
    {
        builder.ToTable(nameof(PhoneArea));

        builder.HasKey(pa => pa.Code);
    }
}
