using TechnicoWebApplication.Dtos;
using TechnicoWebApplication.Models;

namespace TechnicoWebApplication.Mappers;
public class RepairMapper
{
    public Repair GetRepairModel(RepairRequestDto repairRequestDto, PropertyItem propertyItem)
    {
        return new Repair
        {
            Address = repairRequestDto.Address,
            Cost = repairRequestDto.Cost,
            Description = repairRequestDto.Description,
            Status = repairRequestDto.Status, 
            RepairDate = repairRequestDto.RepairDate,
            TypeOfRepair = repairRequestDto.TypeOfRepair,
            PropertyItem = propertyItem
        };
    }

    public RepairResponseDto GetRepairDto(Repair repair)
    {
        return new RepairResponseDto
        {
            Id = repair.Id,
            Address = repair.Address,
            Cost = repair.Cost,
            Description = repair.Description,
            Status = repair.Status,
            RepairDate = repair.RepairDate,
            TypeOfRepair = repair.TypeOfRepair,
            PropertyItemId = repair.PropertyItem.Id,
            Vat = repair.PropertyItem.PropertyOwner.Vat
        };
    }
}
