using FluentValidation;

namespace Application.ExerciseTypes.Commands;

public class DeleteExerciseTypeCommandValidator : AbstractValidator<DeleteExerciseTypeCommand>
{
    public DeleteExerciseTypeCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}