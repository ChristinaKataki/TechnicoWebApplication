using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicoWebApplication.Enums;

namespace TechnicoWebApplication.Models;
public class Repair : SoftDeletableEntity
{
    public long Id { get; set; }
    public DateTime RepairDate { get; set; }
    public TypeOfRepair TypeOfRepair { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; } 
    public RepairStatus Status { get; set; }
    public float Cost { get; set; }
    public required PropertyItem PropertyItem { get; set; }
}
