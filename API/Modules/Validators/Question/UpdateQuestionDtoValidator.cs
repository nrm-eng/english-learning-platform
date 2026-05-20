using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.Question;

public class UpdateQuestionDtoValidator : AbstractValidator<UpdateQuestionDto>
{
    public UpdateQuestionDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Text)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(500);
    }
}