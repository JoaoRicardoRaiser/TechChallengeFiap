using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Infrastructure.Database.Configurations;

public class PhoneAreaConfiguration : IEntityTypeConfiguration<PhoneArea>
{
    public void Configure(EntityTypeBuilder<PhoneArea> builder)
        => builder.HasKey(pac => pac.Code);
}
