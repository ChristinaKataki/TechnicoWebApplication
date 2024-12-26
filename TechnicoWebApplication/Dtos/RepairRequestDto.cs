using TechnicoWebApplication.Models;

namespace TechnicoWebApplication.Dtos;
public class RepairRequestDto
{
    public required string Vat { get; set; }
    public DateTime RepairDate { get; set; }
    public TypeOfRepair TypeOfRepair { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; } ///TODO
    public RepairStatus Status { get; set; } = RepairStatus.Pending;
    public float Cost { get; set; }
}
