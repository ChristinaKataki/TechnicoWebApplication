using FluentValidation;
using System.Text.RegularExpressions;
using TechnicoWebApplication.Dtos;

namespace TechnicoWebApplication.Validators;
public class RepairFiltersValidator : AbstractValidator<RepairFilters>
{
    public RepairFiltersValidator()
    {
        RuleFor(filters => filters.Page)
           .GreaterThanOrEqualTo(1).WithMessage("Page must be greater than or equal to 1.");
        RuleFor(filters => filters.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");
        RuleFor(filters => filters.Vat)
            .Matches("^\\d{9}$").When(filters => filters.Vat != null)
            .WithMessage("Vat must contain 9 digits when provided.");
    }
}

