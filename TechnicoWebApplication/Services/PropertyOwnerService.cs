using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TechnicoWebApplication.Dtos;
using TechnicoWebApplication.Mappers;
using TechnicoWebApplication.Models;
using TechnicoWebApplication.Repositories;

namespace TechnicoWebApplication.Services;
public class PropertyOwnerService
{
    private readonly PropertyOwnerRepository _propertyOwnerRepository;
    private readonly PropertyOwnerMapper _propertyOwnerMapper;

    public PropertyOwnerService(PropertyOwnerRepository propertyOwnerRepository)
    {
        _propertyOwnerRepository = propertyOwnerRepository;
        _propertyOwnerMapper = new PropertyOwnerMapper();
    }

    public async Task<ActionResult<PropertyOwnerResponseDto>> Create(PropertyOwnerRequestDto propertyOwnerRequestDto)
    {
        PropertyOwner propertyOwner = _propertyOwnerMapper.GetPropertyOwnerModel(propertyOwnerRequestDto);

        if (await _propertyOwnerRepository.Read(propertyOwner.Vat) != null)
        {
            return new ConflictObjectResult($"Property owner with vat {propertyOwner.Vat} already exists.");
        }

        PropertyOwner createdPropertyOwner = await _propertyOwnerRepository.Create(propertyOwner);
        PropertyOwnerResponseDto propertyOwnerResponseDto = _propertyOwnerMapper.GetPropertyOwnerDto(createdPropertyOwner);
        return new OkObjectResult(propertyOwnerResponseDto);
    }

    public async Task<ActionResult<PropertyOwnerResponseDto>> Read(string vat)
    {

        PropertyOwner? propertyOwner = await _propertyOwnerRepository.Read(vat);
        if (propertyOwner == null)
        {
            return new NotFoundObjectResult($"There is no owner with vat {vat}.");

        }
        PropertyOwnerResponseDto propertyOwnerResponseDto = _propertyOwnerMapper.GetPropertyOwnerDto(propertyOwner);

        return new OkObjectResult(propertyOwnerResponseDto);
    }

    public async Task<ActionResult<PropertyOwnerResponseDto>> Update(string vat, PropertyOwnerRequestDto propertyOwnerRequestDto)
    {
        PropertyOwner propertyOwner = _propertyOwnerMapper.GetPropertyOwnerModel(propertyOwnerRequestDto);

        if (vat != propertyOwner.Vat)
        {
            return new BadRequestObjectResult($"Vat specified in path ({vat}) is different from vat in request body ({propertyOwner.Vat}).");
        }

        PropertyOwner? updatedOwner = await _propertyOwnerRepository.Update(vat, propertyOwner);

        if (updatedOwner == null)
        {
            return new NotFoundObjectResult($"There is no property owner with {vat}.");
        }

        PropertyOwnerResponseDto propertyOwnerResponseDto = _propertyOwnerMapper.GetPropertyOwnerDto(updatedOwner);

        return new OkObjectResult(propertyOwnerResponseDto);
    }

    public async Task<IActionResult> Delete(string vat)
    {
        return await _propertyOwnerRepository.Delete(vat)
            ? new NoContentResult()
            : new NotFoundObjectResult($"There is no property owner with vat {vat}.");
    }
}
