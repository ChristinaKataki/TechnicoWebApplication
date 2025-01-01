using System.Text.RegularExpressions;

namespace TechnicoWebApplication.Validators;
public static class OwnerValidator
{
    public static bool VatIsNotValid(string vat)
    {
        return string.IsNullOrEmpty(vat) || !Regex.IsMatch(vat, @"^\d{9}$");
    }
}

