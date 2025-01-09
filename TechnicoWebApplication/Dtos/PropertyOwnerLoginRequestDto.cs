using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicoWebApplication.Models;

namespace TechnicoWebApplication.Dtos;
public class PropertyOwnerLoginRequestDto
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}
