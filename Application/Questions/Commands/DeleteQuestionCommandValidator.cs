using FluentValidation;

namespace Application.Questions.Commands;

public class DeleteQuestionCommandValidator : AbstractValidator<DeleteQuestionCommand>
{
    public DeleteQuestionCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}