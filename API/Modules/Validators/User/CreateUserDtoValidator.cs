using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.User;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255);
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);
    }
}