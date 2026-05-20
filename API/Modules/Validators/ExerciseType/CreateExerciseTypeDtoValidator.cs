using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.ExerciseType;

public class CreateExerciseTypeDtoValidator : AbstractValidator<CreateExerciseTypeDto>
{
    public CreateExerciseTypeDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50);
    }
}