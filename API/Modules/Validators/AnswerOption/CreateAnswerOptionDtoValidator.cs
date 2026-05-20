using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.AnswerOption;

public class CreateAnswerOptionDtoValidator : AbstractValidator<CreateAnswerOptionDto>
{
    public CreateAnswerOptionDtoValidator()
    {
        RuleFor(x => x.QuestionId).GreaterThan(0);
        RuleFor(x => x.Text)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(300);
    }
}