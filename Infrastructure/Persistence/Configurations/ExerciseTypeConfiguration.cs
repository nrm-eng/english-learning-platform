using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ExerciseTypeConfiguration : IEntityTypeConfiguration<ExerciseType>
{
    public void Configure(EntityTypeBuilder<ExerciseType> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.HasData(
            new { Id = 1, Name = "Reading" },
            new { Id = 2, Name = "Grammar" },
            new { Id = 3, Name = "Vocabulary" }
        );
    }
}