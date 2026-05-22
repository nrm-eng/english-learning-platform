using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class LevelConfiguration : IEntityTypeConfiguration<Level>
{
    public void Configure(EntityTypeBuilder<Level> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CefrCode)
            .HasConversion<string>()
            .HasColumnType("varchar(10)")
            .IsRequired();
        builder.Property(x => x.Name)
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.HasData(
            new { Id = 1, CefrCode = CefrLevel.A1, Name = "Beginner" },
            new { Id = 2, CefrCode = CefrLevel.A2, Name = "Elementary" },
            new { Id = 3, CefrCode = CefrLevel.B1, Name = "Intermediate" },
            new { Id = 4, CefrCode = CefrLevel.B2, Name = "Upper-Intermediate" },
            new { Id = 5, CefrCode = CefrLevel.C1, Name = "Advanced" }
        );
    }
}