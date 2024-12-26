using TechnicoWebApplication.Dtos;
using TechnicoWebApplication.Models;

namespace TechnicoWebApplication.Mappers;

public class PropertyItemMapper
{
    public PropertyItem GetPropertyItemModel(PropertyItemRequestDto propertyItemRequestDto, PropertyOwner propertyOwner)
    {
        return new PropertyItem
        {
            Id = propertyItemRequestDto.Id,
            PropertyType = propertyItemRequestDto.PropertyType,
            Address = propertyItemRequestDto.Address,
            ConstructionYear = propertyItemRequestDto.ConstructionYear,
            PropertyOwner = propertyOwner
        };
    }

    public PropertyItemResponseDto GetPropertyItemDto(PropertyItem propertyItem)
    {
        return new PropertyItemResponseDto
        {
            Id = propertyItem.Id,
            PropertyType = propertyItem.PropertyType,
            Address = propertyItem.Address,
            ConstructionYear = propertyItem.ConstructionYear,
            Vat = propertyItem.PropertyOwner.Vat
        };
    }
}
