using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicoWebApplication.Models;

namespace TechnicoWebApplication.Dtos;
public class PropertyOwnerCreationRequestDto : PropertyOwnerUpdateRequestDto
{
    public string? Vat { get; set; }
    public string? Password { get; set; }
}
