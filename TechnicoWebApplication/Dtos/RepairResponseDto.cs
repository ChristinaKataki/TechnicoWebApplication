using TechnicoWebApplication.Enums;

namespace TechnicoWebApplication.Dtos;
public class RepairResponseDto
{
    public required string Vat { get; set; }
    public long Id { get; set; }
    public DateTime RepairDate { get; set; }
    public TypeOfRepair TypeOfRepair { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public RepairStatus Status { get; set; }
    public float Cost { get; set; }
}
