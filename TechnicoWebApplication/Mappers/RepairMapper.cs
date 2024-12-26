using TechnicoWebApplication.Dtos;
using TechnicoWebApplication.Models;

namespace TechnicoWebApplication.Mappers;
public class RepairMapper
{
    public Repair GetRepairModel(RepairRequestDto repairRequestDto, PropertyOwner propertyOwner)
    {
        return new Repair
        {
            Address = repairRequestDto.Address,
            Cost = repairRequestDto.Cost,
            Description = repairRequestDto.Description,
            Status = repairRequestDto.Status, 
            RepairDate = repairRequestDto.RepairDate,
            TypeOfRepair = repairRequestDto.TypeOfRepair,
            PropertyOwner = propertyOwner
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
            Vat = repair.PropertyOwner.Vat
        };
    }
}
