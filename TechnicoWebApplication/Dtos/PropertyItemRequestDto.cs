using TechnicoWebApplication.Enums;

namespace TechnicoWebApplication.Dtos;
public class PropertyItemRequestDto
{
    public required string Vat { get; set; }
    public required string Id { get; set; }
    public string? Address { get; set; }
    public int ConstructionYear { get; set; }
    public PropertyType PropertyType { get; set; }
}