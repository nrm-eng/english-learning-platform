using FluentValidation;

namespace Application.PlacementTests.Commands;

public class SubmitPlacementTestCommandValidator : AbstractValidator<SubmitPlacementTestCommand>
{
    public SubmitPlacementTestCommandValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.LevelId).GreaterThan(0);
        RuleFor(x => x.Score).GreaterThanOrEqualTo(0);
        RuleFor(x => x.MaxScore).GreaterThan(0);
        RuleFor(x => x.Score)
            .LessThanOrEqualTo(x => x.MaxScore)
            .WithMessage("Score cannot be greater than MaxScore");
    }
}