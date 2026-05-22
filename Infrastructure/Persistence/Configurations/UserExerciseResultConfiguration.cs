using Domain.Entities;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserExerciseResultConfiguration : IEntityTypeConfiguration<UserExerciseResult>
{
    public void Configure(EntityTypeBuilder<UserExerciseResult> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId)
            .IsRequired();
        builder.Property(x => x.ExerciseId)
            .IsRequired();
        builder.Property(x => x.Score)
            .IsRequired();
        builder.Property(x => x.MaxScore)
            .IsRequired();
        builder.Property(x => x.CompletedAt)
            .HasConversion(new DateTimeUtcConverter())
            .IsRequired();

        builder.HasIndex(x => new { x.UserId, x.ExerciseId })
            .IsUnique();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Exercise)
            .WithMany()
            .HasForeignKey(x => x.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}