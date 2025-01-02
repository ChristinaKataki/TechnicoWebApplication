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
        if (OwnerValidator.VatIsNotValid(propertyOwnerRequestDto.Vat))
        {
            return new BadRequestObjectResult($"The Vat [{propertyOwnerRequestDto.Vat}] is not valid.");
        }

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
        if (OwnerValidator.VatIsNotValid(vat))
        {
            return new BadRequestObjectResult($"The Vat [{vat}] is not valid.");
        }

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
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return new BadRequestObjectResult("Email and password are required.");
        }

        PropertyOwner? propertyOwner = await _propertyOwnerRepository.ReadByEmail(email);
        if (propertyOwner == null || !BCrypt.Net.BCrypt.Verify(password, propertyOwner.Password))
        {
            return new UnauthorizedObjectResult("Invalid credentials.");
        }

        string jwtToken = TokenProvider.Create(propertyOwner);
        PropertyOwnerLoginResponseDto propertyOwnerLoginResponseDto = _propertyOwnerMapper.GetPropertyOwnerLoginResponseDto(propertyOwner, jwtToken);

        return new OkObjectResult(propertyOwnerLoginResponseDto);
    }

    public async Task<ActionResult<PropertyOwnerResponseDto>> Update(string vat, PropertyOwnerRequestDto propertyOwnerRequestDto)
    {
        if (OwnerValidator.VatIsNotValid(propertyOwnerRequestDto.Vat))
        {
            return new BadRequestObjectResult($"The Vat [{propertyOwnerRequestDto.Vat}] is not valid.");
        }

        if (OwnerValidator.VatIsNotValid(vat))
        {
            return new BadRequestObjectResult($"The Vat [{vat}] is not valid.");
        }

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
        if (OwnerValidator.VatIsNotValid(vat))
        {
            return new BadRequestObjectResult($"The Vat [{vat}] is not valid.");
        }

        return await _propertyOwnerRepository.Delete(vat)
            ? new NoContentResult()
            : new NotFoundObjectResult($"There is no property owner with vat {vat}.");
    }
}
