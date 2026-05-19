using FluentValidation;

namespace Application.AnswerOptions.Commands;

public class UpdateAnswerOptionCommandValidator : AbstractValidator<UpdateAnswerOptionCommand>
{
    public UpdateAnswerOptionCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Text)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(300);
    }
}