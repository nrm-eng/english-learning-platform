using FluentValidation;

namespace Application.AnswerOptions.Commands;

public class DeleteAnswerOptionCommandValidator : AbstractValidator<DeleteAnswerOptionCommand>
{
    public DeleteAnswerOptionCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}