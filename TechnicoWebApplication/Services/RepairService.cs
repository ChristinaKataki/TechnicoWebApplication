using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicoWebApplication.Dtos;
using TechnicoWebApplication.Mappers;
using TechnicoWebApplication.Models;
using TechnicoWebApplication.Repositories;
using TechnicoWebApplication.Validators;

namespace TechnicoWebApplication.Services;
public class RepairService
{
    private readonly RepairRepository _repairRepository;
    private readonly RepairMapper _repairMapper;
    private readonly PropertyOwnerRepository _propertyOwnerRepository;
    private readonly PropertyItemRepository _propertyItemRepository;

    public RepairService(RepairRepository repairRepository, PropertyOwnerRepository propertyOwnerRepository, PropertyItemRepository propertyItemRepository)
    {
        _repairRepository = repairRepository;
        _repairMapper = new RepairMapper();
        _propertyOwnerRepository = propertyOwnerRepository;
        _propertyItemRepository = propertyItemRepository;
    }

    public async Task<ActionResult<RepairResponseDto>> Create(RepairRequestDto repairRequestDto)
    {
        PropertyItem? propertyItem = await _propertyItemRepository.Read(repairRequestDto.PropertyItemId);
        if (propertyItem == null)
        {
            return new NotFoundObjectResult($"There is no property with the provided Id ({repairRequestDto.PropertyItemId}).");
        }

        Repair repair = _repairMapper.GetRepairModel(repairRequestDto, propertyItem);

        Repair createdRepair = await _repairRepository.Create(repair);
        RepairResponseDto repairResponseDto = _repairMapper.GetRepairDto(createdRepair);
        return new OkObjectResult(repairResponseDto);
    }

    public async Task<ActionResult<RepairResponseDto>> Read(long id)
    {

        Repair? repair = await _repairRepository.Read(id);
        if (repair == null)
        {
            return new NotFoundObjectResult($"There is no repair with id {id}.");

        }
        RepairResponseDto repairResponseDto = _repairMapper.GetRepairDto(repair);

        return new OkObjectResult(repairResponseDto);
    }

    public async Task<ActionResult<List<RepairResponseDto>>> FindByOwner(string vat)
    {
        if (OwnerValidator.VatIsNotValid(vat))
        {
            return new BadRequestObjectResult($"The Vat [{vat}] is not valid.");
        }

        PropertyOwner? propertyOwner = await _propertyOwnerRepository.Read(vat);
        if (propertyOwner == null)
        {
            return new NotFoundObjectResult($"There is no owner with the provided vat ({vat}).");
        }

        List<Repair> repairs = await _repairRepository.FindByOwner(vat);
        List<RepairResponseDto> repairResponseDtos = repairs.ConvertAll(repair => _repairMapper.GetRepairDto(repair));

        return new OkObjectResult(repairResponseDtos);
    }

    public async Task<ActionResult<RepairResponseDto>> Update(long id, RepairRequestDto repairRequestDto)
    {
        PropertyItem? propertyItem = await _propertyItemRepository.Read(repairRequestDto.PropertyItemId);
        if (propertyItem == null)
        {
            return new NotFoundObjectResult($"There is no property with the provided Id ({repairRequestDto.PropertyItemId}).");
        }

        Repair repair = _repairMapper.GetRepairModel(repairRequestDto, propertyItem);
        repair.Id = id;

        Repair? updatedRepair = await _repairRepository.Update(id, repair);

        if (updatedRepair == null)
        {
            return new NotFoundObjectResult($"There is no repair with {id}.");
        }

        RepairResponseDto repairResponseDto = _repairMapper.GetRepairDto(updatedRepair);

        return new OkObjectResult(repairResponseDto);
    }

    public async Task<IActionResult> Delete(long id)
    {
        return await _repairRepository.Delete(id)
            ? new NoContentResult()
            : new NotFoundObjectResult($"There is no repair with id {id}.");
    }

    public async Task<IActionResult> SoftDelete(long id)
    {

        Repair? repair = await _repairRepository.Read(id);
        if (repair == null)
        {
            return new NotFoundObjectResult($"There is no repair with id {id}.");
        }

        repair.IsDeleted = true;

        await _repairRepository.Update(id, repair);

        return new NoContentResult();
    }

    public async Task<string?> GetOwnerOfRepair(long id)
    {
        Repair? repair= await _repairRepository.Read(id);
        return repair?.PropertyItem?.PropertyOwner?.Vat;
    }

    public async Task<IActionResult> Search(RepairFilters filters)
    {
        return await _repairRepository.ReadWithFilters(filters);
    }
}

