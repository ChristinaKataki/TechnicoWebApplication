using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging.Signing;
using System.Security.Claims;
using System.Text;
using TechnicoWebApplication.Models;
using TechnicoWebApplication.Dtos;

namespace TechnicoWebApplication.Helpers;
internal sealed class TokenProvider
{
    public static string Create(PropertyOwner propertyOwnerResponseDto)
    {
        string secretKey = "SomeLongSecretKeyForSigningThatIs256BitsLong";
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim("vat", propertyOwnerResponseDto.Vat),
                new Claim("userType", propertyOwnerResponseDto.UserType.ToString())
            ]),
            Issuer = "Technico",
            Audience = "Technico",
            SigningCredentials = credentials
        };

        var handler = new JsonWebTokenHandler();
        string token = handler.CreateToken(tokenDescriptor);
        return token;
    }
}
