using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TechnicoWebApplication.Dtos;
using TechnicoWebApplication.Mappers;
using TechnicoWebApplication.Models;
using TechnicoWebApplication.Repositories;
using TechnicoWebApplication.Validators;
using Microsoft.AspNetCore.Http;
using NuGet.Common;
using TechnicoWebApplication.Helpers;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using FluentValidation.Results;
using System.ComponentModel.DataAnnotations;

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

    public async Task<ActionResult<PropertyOwnerResponseDto>> Create(PropertyOwnerCreationRequestDto propertyOwnerRequestDto)
    {
        PropertyOwner propertyOwner = _propertyOwnerMapper.GetPropertyOwnerModel(propertyOwnerRequestDto);

        if (await _propertyOwnerRepository.Read(propertyOwner.Vat) != null)
        {
            return new ConflictObjectResult($"Property owner with vat {propertyOwner.Vat} already exists.");
        }

        if (await _propertyOwnerRepository.ReadByEmail(propertyOwner.Email) != null)
        {
            return new ConflictObjectResult($"Email {propertyOwner.Email} has already been used.");
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

    public async Task<ActionResult<PropertyOwnerLoginResponseDto>> ReadByEmailAndPassword(string email, string password)
    {
        PropertyOwner? propertyOwner = await _propertyOwnerRepository.ReadByEmail(email);
        if (propertyOwner == null || !BCrypt.Net.BCrypt.Verify(password, propertyOwner.Password))
        {
            return new UnauthorizedObjectResult("Invalid credentials.");
        }

        string jwtToken = TokenProvider.Create(propertyOwner);
        PropertyOwnerLoginResponseDto propertyOwnerLoginResponseDto = _propertyOwnerMapper.GetPropertyOwnerLoginResponseDto(propertyOwner, jwtToken);

        return new OkObjectResult(propertyOwnerLoginResponseDto);
    }

    public async Task<ActionResult<PropertyOwnerResponseDto>> Update(string vat, PropertyOwnerUpdateRequestDto propertyOwnerRequestDto)
    {
        PropertyOwner? propertyOwner = await _propertyOwnerRepository.Read(vat);

        if (propertyOwner == null)
        {
            return new NotFoundObjectResult($"There is no property owner with {vat}.");
        }

        propertyOwner.Name = propertyOwnerRequestDto.Name;
        propertyOwner.Surname = propertyOwnerRequestDto.Surname;
        propertyOwner.PhoneNumber = propertyOwnerRequestDto.PhoneNumber;
        propertyOwner.Address = propertyOwnerRequestDto.Address;
        propertyOwner.Email = propertyOwnerRequestDto.Email;

        PropertyOwner? updatedOwner = await _propertyOwnerRepository.Update(vat, propertyOwner);

        PropertyOwnerResponseDto propertyOwnerResponseDto = _propertyOwnerMapper.GetPropertyOwnerDto(updatedOwner);

        return new OkObjectResult(propertyOwnerResponseDto);
    }

    public async Task<IActionResult> Delete(string vat)
    {
        return await _propertyOwnerRepository.Delete(vat)
            ? new NoContentResult()
            : new NotFoundObjectResult($"There is no property owner with vat {vat}.");
    }

    public async Task<IActionResult> SoftDelete(string vat)
    {

        PropertyOwner? propertyOwner = await _propertyOwnerRepository.Read(vat);
        if (propertyOwner == null)
        {
            return new NotFoundObjectResult($"There is no property owner with vat {vat}.");
        }

        propertyOwner.IsDeleted = true;
        propertyOwner.PropertyItems.ForEach(item =>
        {
            item.IsDeleted = true;
            item.Repairs.ForEach(repair => repair.IsDeleted = true);
        });

        await _propertyOwnerRepository.Update(vat, propertyOwner);

        return new NoContentResult();
    }

    public async Task<IActionResult> Search(PropertyOwnerFilters filters)
    {
        PageResults<PropertyOwner> results = await _propertyOwnerRepository.ReadWithFilters(filters);

        return new OkObjectResult(new PageResults<PropertyOwnerResponseDto>
        {
            TotalCount = results.TotalCount,
            Page = results.Page,
            PageSize = filters.PageSize,
            Elements = results.Elements.ConvertAll(owner => _propertyOwnerMapper.GetPropertyOwnerDto(owner))
        });
    }
}
