using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;
using System.Text.RegularExpressions;
using TechnicoWebApplication.Dtos;

namespace TechnicoWebApplication.Validators;
public class LoginValidator : AbstractValidator<PropertyOwnerLoginRequestDto>
{
    public LoginValidator()
    {
        RuleFor(loginRequest => loginRequest.Email).EmailAddress().NotNull().WithMessage("Email must be provided.");
        RuleFor(loginRequest => loginRequest.Password).NotNull().WithMessage("Password must be provided.");
    }
}

