using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.ExerciseType;

public class UpdateExerciseTypeDtoValidator : AbstractValidator<UpdateExerciseTypeDto>
{
    public UpdateExerciseTypeDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50);
    }
}