using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechChallenge.GetContact.Domain.Entities;

namespace TechChallenge.GetContact.Infrastructure.Database.Configurations;

public class PhoneAreaConfiguration : IEntityTypeConfiguration<PhoneArea>
{
    public void Configure(EntityTypeBuilder<PhoneArea> builder)
    {
        builder.ToTable(nameof(PhoneArea));

        builder.HasKey(pa => pa.Code);
    }
}
