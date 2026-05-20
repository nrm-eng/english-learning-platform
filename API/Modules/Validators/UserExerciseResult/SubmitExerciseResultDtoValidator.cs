using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.UserExerciseResult;

public class SubmitExerciseResultDtoValidator : AbstractValidator<SubmitExerciseResultDto>
{
    public SubmitExerciseResultDtoValidator()
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