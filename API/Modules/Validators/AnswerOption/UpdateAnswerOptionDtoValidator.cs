using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.AnswerOption;

public class UpdateAnswerOptionDtoValidator : AbstractValidator<UpdateAnswerOptionDto>
{
    public UpdateAnswerOptionDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Text)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(300);
    }
}