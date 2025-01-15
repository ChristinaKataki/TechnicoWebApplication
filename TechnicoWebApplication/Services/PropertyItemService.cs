using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicoWebApplication.Dtos;
using TechnicoWebApplication.Helpers;
using TechnicoWebApplication.Mappers;
using TechnicoWebApplication.Models;
using TechnicoWebApplication.Repositories;
using TechnicoWebApplication.Validators;

namespace TechnicoWebApplication.Services;
public class PropertyItemService
{
    private readonly PropertyItemRepository _propertyItemRepository;
    private readonly PropertyItemMapper _propertyItemMapper;
    private readonly PropertyOwnerRepository _propertyOwnerRepository;

    public PropertyItemService(PropertyItemRepository propertyItemRepository, PropertyOwnerRepository propertyOwnerRepository)
    {
        _propertyItemRepository = propertyItemRepository;
        _propertyItemMapper = new PropertyItemMapper();
        _propertyOwnerRepository = propertyOwnerRepository;
    }

    public async Task<ActionResult<PropertyItemResponseDto>> Create(PropertyItemRequestDto propertyItemRequestDto)
    {
        PropertyOwner? propertyOwner = await _propertyOwnerRepository.Read(propertyItemRequestDto.Vat);
        if (propertyOwner == null)
        {
            return new NotFoundObjectResult($"There is no owner with the provided vat ({propertyItemRequestDto.Vat}).");
        }

        PropertyItem propertyItem = _propertyItemMapper.GetPropertyItemModel(propertyItemRequestDto, propertyOwner);

        if (await _propertyItemRepository.Read(propertyItem.Id) != null)
        {
            return new ConflictObjectResult($"Property item with id {propertyItem.Id} already exists.");
        }

        PropertyItem? softDeletedItem = await _propertyItemRepository.ReadSoftDeleted(propertyItem.Id);
        if (softDeletedItem != null)
        {
            if (softDeletedItem.PropertyOwner.Vat != propertyItem.PropertyOwner.Vat)
            {
                return new ConflictObjectResult($"Deactivated property item with id {propertyItem.Id} belongs to another owner.");
            } 
            else 
            {
                return new ConflictObjectResult($"Deactivated property item with id {propertyItem.Id} exists for owner with vat {propertyItem.PropertyOwner.Vat}.");
            }
        }

        PropertyItem createdPropertyItem = await _propertyItemRepository.Create(propertyItem);
        PropertyItemResponseDto propertyItemResponseDto = _propertyItemMapper.GetPropertyItemDto(createdPropertyItem);
        return new OkObjectResult(propertyItemResponseDto);
    }

    public async Task<ActionResult<PropertyItemResponseDto>> Read(string id)
    {

        PropertyItem? propertyItem = await _propertyItemRepository.Read(id);
        if (propertyItem == null)
        {
            return new NotFoundObjectResult($"There is no item with id {id}.");

        }
        PropertyItemResponseDto propertyItemResponseDto = _propertyItemMapper.GetPropertyItemDto(propertyItem);

        return new OkObjectResult(propertyItemResponseDto);
    }

    public async Task<ActionResult<List<PropertyItemResponseDto>>> FindByOwner(string vat)
    {
        PropertyOwner? propertyOwner = await _propertyOwnerRepository.Read(vat);
        if (propertyOwner == null)
        {
            return new NotFoundObjectResult($"There is no owner with the provided vat ({vat}).");
        }

        List<PropertyItem> propertyItems = await _propertyItemRepository.FindByOwner(vat);
        List<PropertyItemResponseDto> propertyItemResponseDtos = propertyItems.ConvertAll(item => _propertyItemMapper.GetPropertyItemDto(item));

        return new OkObjectResult(propertyItemResponseDtos);
    }

    public async Task<ActionResult<PropertyItemResponseDto>> Update(string id, PropertyItemRequestDto propertyItemRequestDto)
    {
        if (id != propertyItemRequestDto.Id)
        {
            return new BadRequestObjectResult($"Item id specified in path ({id}) is different from item id in request body ({propertyItemRequestDto.Id}).");
        }

        PropertyOwner? propertyOwner = await _propertyOwnerRepository.Read(propertyItemRequestDto.Vat);
        if (propertyOwner == null)
        {
            return new NotFoundObjectResult($"There is no owner with the provided vat ({propertyItemRequestDto.Vat}).");
        }

        PropertyItem propertyItem = _propertyItemMapper.GetPropertyItemModel(propertyItemRequestDto, propertyOwner);

        PropertyItem? updatedItem = await _propertyItemRepository.Update(id, propertyItem);

        if (updatedItem == null)
        {
            return new NotFoundObjectResult($"There is no property item with {id}.");
        }

        PropertyItemResponseDto propertyItemResponseDto = _propertyItemMapper.GetPropertyItemDto(updatedItem);

        return new OkObjectResult(propertyItemResponseDto);
    }

    public async Task<IActionResult> Delete(string id)
    {
        return await _propertyItemRepository.Delete(id)
            ? new NoContentResult()
            : new NotFoundObjectResult($"There is no property item with id {id}.");
    }

    public async Task<IActionResult> SoftDelete(string id)
    {

        PropertyItem? propertyItem = await _propertyItemRepository.Read(id);
        if (propertyItem == null)
        {
            return new NotFoundObjectResult($"There is no property item with id {id}.");
        }

        propertyItem.IsDeleted = true;
        propertyItem.Repairs.ForEach(repair => repair.IsDeleted = true);

        await _propertyItemRepository.Update(id, propertyItem);

        return new NoContentResult();
    }

    public async Task<string?> GetOwnerOfItem(string id)
    {
        PropertyItem? propertyItem = await _propertyItemRepository.Read(id);
        return propertyItem?.PropertyOwner?.Vat;
    }

    public async Task<IActionResult> Search(PropertyItemFilters filters)
    {
        PageResults<PropertyItem> results = await _propertyItemRepository.ReadWithFilters(filters);

        return new OkObjectResult(new PageResults<PropertyItemResponseDto>
        {
            TotalCount = results.TotalCount,
            Page = results.Page,
            PageSize = filters.PageSize,
            Elements = results.Elements.ConvertAll(item => _propertyItemMapper.GetPropertyItemDto(item))
        });
    }
}

