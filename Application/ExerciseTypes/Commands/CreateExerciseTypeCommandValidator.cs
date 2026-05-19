using FluentValidation;

namespace Application.ExerciseTypes.Commands;

public class CreateExerciseTypeCommandValidator : AbstractValidator<CreateExerciseTypeCommand>
{
    public CreateExerciseTypeCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50);
    }
}