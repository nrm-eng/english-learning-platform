using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.Exercise;

public class UpdateExerciseDtoValidator : AbstractValidator<UpdateExerciseDto>
{
    public UpdateExerciseDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.LevelId).GreaterThan(0);
        RuleFor(x => x.TypeId).GreaterThan(0);
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(200);
        RuleFor(x => x.Content)
            .NotEmpty();
    }
}