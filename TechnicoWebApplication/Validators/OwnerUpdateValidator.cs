using FluentValidation;
using System.Text.RegularExpressions;
using TechnicoWebApplication.Dtos;

namespace TechnicoWebApplication.Validators;
public class OwnerUpdateValidator : AbstractValidator<PropertyOwnerUpdateRequestDto>
{
    public OwnerUpdateValidator()
    {
        RuleFor(o => o.Email).NotNull().EmailAddress();
    }
}

