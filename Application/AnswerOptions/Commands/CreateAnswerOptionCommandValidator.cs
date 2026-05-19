using FluentValidation;

namespace Application.AnswerOptions.Commands;

public class CreateAnswerOptionCommandValidator : AbstractValidator<CreateAnswerOptionCommand>
{
    public CreateAnswerOptionCommandValidator()
    {
        RuleFor(x => x.QuestionId).GreaterThan(0);
        RuleFor(x => x.Text)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(300);
    }
}