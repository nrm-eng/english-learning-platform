using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators.PlacementTest;

public class SubmitPlacementTestDtoValidator : AbstractValidator<SubmitPlacementTestDto>
{
    public SubmitPlacementTestDtoValidator()
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