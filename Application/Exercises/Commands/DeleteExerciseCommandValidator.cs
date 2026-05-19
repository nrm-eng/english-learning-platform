using FluentValidation;

namespace Application.Exercises.Commands;

public class DeleteExerciseCommandValidator : AbstractValidator<DeleteExerciseCommand>
{
    public DeleteExerciseCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}