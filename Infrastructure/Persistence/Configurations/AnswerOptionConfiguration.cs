using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class AnswerOptionConfiguration : IEntityTypeConfiguration<AnswerOption>
{
    public void Configure(EntityTypeBuilder<AnswerOption> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.QuestionId)
            .IsRequired();
        builder.Property(x => x.Text)
            .HasColumnType("varchar(300)")
            .IsRequired();
        builder.Property(x => x.IsCorrect)
            .IsRequired();
    }
}