using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.Question;

public class CreateQuestionDtoValidator : AbstractValidator<CreateQuestionDto>
{
    public CreateQuestionDtoValidator()
    {
        RuleFor(x => x.ExerciseId).GreaterThan(0);
        RuleFor(x => x.Text)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(500);
    }
}