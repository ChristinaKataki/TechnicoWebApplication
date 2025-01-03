using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicoWebApplication.Enums;

namespace TechnicoWebApplication.Models;
public class PropertyItem : SoftDeletableEntity
{
    public required string Id { get; set; }
    public string? Address { get; set; } 
    public int ConstructionYear { get; set; }
    public PropertyType PropertyType { get; set; }
    public required PropertyOwner PropertyOwner { get; set; }
}