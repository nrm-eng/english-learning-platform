using FluentValidation;

namespace Application.UserExerciseResults.Commands;

public class SubmitExerciseResultCommandValidator : AbstractValidator<SubmitExerciseResultCommand>
{
    public SubmitExerciseResultCommandValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.ExerciseId).GreaterThan(0);
        RuleFor(x => x.Score).GreaterThanOrEqualTo(0);
        RuleFor(x => x.MaxScore).GreaterThan(0);
        RuleFor(x => x.Score)
            .LessThanOrEqualTo(x => x.MaxScore)
            .WithMessage("Score cannot be greater than MaxScore");
    }
}