﻿using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicoWebApplication.Dtos;
using TechnicoWebApplication.Enums;
using TechnicoWebApplication.Models;

namespace TechnicoWebApplication.Mappers;
public class PropertyOwnerMapper
{
    public PropertyOwner GetPropertyOwnerModel(PropertyOwnerCreationRequestDto propertyOwnerRequestDto)
    {
        return new PropertyOwner
        {
            Vat = propertyOwnerRequestDto.Vat,
            Name = propertyOwnerRequestDto.Name,
            Surname = propertyOwnerRequestDto.Surname,
            Address = propertyOwnerRequestDto.Address,
            PhoneNumber = propertyOwnerRequestDto.PhoneNumber,
            Email = propertyOwnerRequestDto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(propertyOwnerRequestDto.Password),
            UserType = UserType.User,
            PropertyItems = []
        };
    }

    public PropertyOwnerResponseDto GetPropertyOwnerDto(PropertyOwner propertyOwner)
    {
        return new PropertyOwnerResponseDto
        {
            Vat = propertyOwner.Vat,
            Name = propertyOwner.Name,
            Surname = propertyOwner.Surname,
            Address = propertyOwner.Address,
            PhoneNumber = propertyOwner.PhoneNumber,
            Email = propertyOwner.Email,
        };
    }

    public PropertyOwnerLoginResponseDto GetPropertyOwnerLoginResponseDto(PropertyOwner propertyOwner, string token)
    {
        return new PropertyOwnerLoginResponseDto
        {
            Vat = propertyOwner.Vat,
            Name = propertyOwner.Name,
            Surname = propertyOwner.Surname,
            Address = propertyOwner.Address,
            PhoneNumber = propertyOwner.PhoneNumber,
            Email = propertyOwner.Email,
            UserType = propertyOwner.UserType,
            Token = token
        };
    }
}
