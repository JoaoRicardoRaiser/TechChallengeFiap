using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Infrastructure.Database.Configurations;

[ExcludeFromCodeCoverage]
public class PhoneAreaConfiguration : IEntityTypeConfiguration<PhoneArea>
{
    public void Configure(EntityTypeBuilder<PhoneArea> builder)
        => builder.HasKey(pac => pac.Code);
}
