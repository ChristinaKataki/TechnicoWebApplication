using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicoWebApplication.Enums;
using TechnicoWebApplication.Models;

namespace TechnicoWebApplication.Dtos;
public class PropertyOwnerLoginResponseDto
{
    public required string Token { get; set; }
    public required PropertyOwnerResponseDto PropertyOwner { get; set; }

    public required UserType UserType { get; set; }
}
