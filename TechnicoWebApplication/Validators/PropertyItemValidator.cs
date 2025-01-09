using FluentValidation;
using System.Text.RegularExpressions;
using TechnicoWebApplication.Dtos;

namespace TechnicoWebApplication.Validators;
public class PropertyItemValidator : AbstractValidator<PropertyItemRequestDto>
{
    public PropertyItemValidator()
    {
        RuleFor(item => item.Id)
            .NotEmpty()
            .WithMessage("Id of property item (as stated in E9) is required.");
        RuleFor(item => item.Vat)
            .NotEmpty()
            .Matches("^\\d{9}$").WithMessage("Vat must contain 9 digits.");
        RuleFor(item => item.ConstructionYear)
            .InclusiveBetween(1800, DateTime.Now.Year)
            .WithMessage($"Year of construction must be between 1800 and {DateTime.Now.Year}.");
    }
}

