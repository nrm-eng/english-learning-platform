using FluentValidation;

namespace Application.PlacementTests.Commands;

public class CreatePlacementTestCommandValidator : AbstractValidator<CreatePlacementTestCommand>
{
    public CreatePlacementTestCommandValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.LevelId).GreaterThan(0);
        RuleFor(x => x.Score).GreaterThanOrEqualTo(0);
        RuleFor(x => x.MaxScore).GreaterThan(0);
    }
}