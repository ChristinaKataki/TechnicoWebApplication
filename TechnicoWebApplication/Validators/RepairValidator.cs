using FluentValidation;
using System.Text.RegularExpressions;
using TechnicoWebApplication.Dtos;

namespace TechnicoWebApplication.Validators;
public class RepairValidator : AbstractValidator<RepairRequestDto>
{
    public RepairValidator()
    {
        RuleFor(repair => repair.PropertyItemId)
            .NotEmpty()
            .WithMessage("Id of property item (as stated in E9) is required.");

        RuleFor(repair => repair.Cost)
           .GreaterThanOrEqualTo(0)
           .WithMessage("Cost cannot be negative.");
    }
}

