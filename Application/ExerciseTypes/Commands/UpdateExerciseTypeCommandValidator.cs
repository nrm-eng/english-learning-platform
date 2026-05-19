using FluentValidation;

namespace Application.ExerciseTypes.Commands;

public class UpdateExerciseTypeCommandValidator : AbstractValidator<UpdateExerciseTypeCommand>
{
    public UpdateExerciseTypeCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50);
    }
}