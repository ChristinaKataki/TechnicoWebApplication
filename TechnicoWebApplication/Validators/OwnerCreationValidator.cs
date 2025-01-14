using FluentValidation;
using System.Text.RegularExpressions;
using TechnicoWebApplication.Dtos;

namespace TechnicoWebApplication.Validators;
public class OwnerCreationValidator : AbstractValidator<PropertyOwnerCreationRequestDto>
{
    public OwnerCreationValidator()
    {
        Include(new OwnerUpdateValidator());
        RuleFor(o => o.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
        RuleFor(o => o.Vat)
            .NotEmpty()
            .Matches("^\\d{9}$").WithMessage("Vat must contain 9 digits.");
    }
}

