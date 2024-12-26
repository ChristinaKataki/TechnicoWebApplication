using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicoWebApplication.Models;
public class Repair : IEntity<long>
{
    public long Id { get; set; }
    public DateTime RepairDate { get; set; }
    public TypeOfRepair TypeOfRepair { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; } ///TODO
    public RepairStatus Status { get; set; }
    public float Cost { get; set; }
    public required PropertyOwner PropertyOwner { get; set; }
}
