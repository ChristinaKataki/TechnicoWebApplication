﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicoWebApplication.Context;
using TechnicoWebApplication.Dtos;
using TechnicoWebApplication.Models;
using TechnicoWebApplication.Services;
using TechnicoWebApplication.Helpers;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using TechnicoWebApplication.Enums;
using System.ComponentModel.DataAnnotations;

namespace TechnicoWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyOwnersController : ControllerBase
    {
        private readonly PropertyOwnerService _propertyOwnerService;
        private readonly PropertyItemService _propertyItemService;
        private readonly RepairService _repairService;

        public PropertyOwnersController(PropertyOwnerService propertyOwnerService, PropertyItemService propertyItemService, RepairService repairService)
        {
            _propertyOwnerService = propertyOwnerService;
            _propertyItemService = propertyItemService;
            _repairService = repairService;
        }

        // CREATE
        [HttpPost]
        public async Task<ActionResult<PropertyOwnerResponseDto>> PostPropertyOwner(PropertyOwnerCreationRequestDto propertyOwnerRequestDto)
        {
            var response = await _propertyOwnerService.Create(propertyOwnerRequestDto);
            return response;
        }

        // GET
        [HttpGet("{vat}")]
        [Authorize]
        public async Task<ActionResult<PropertyOwnerResponseDto>> GetPropertyOwner([RegularExpression(@"^\d{9}$", ErrorMessage = "Vat must contain 9 digits.")] string vat)
        {
            if (User.FindFirst("vat")?.Value != vat && User.FindFirst("userType")?.Value != UserType.Admin.ToString())
            {
                return Forbid();
            }

            var response = await _propertyOwnerService.Read(vat);
            return response;
        }

        // GET
        [HttpGet("{vat}/PropertyItems")]
        [Authorize]
        public async Task<ActionResult<List<PropertyItemResponseDto>>> GetItemsOfOwner([RegularExpression(@"^\d{9}$", ErrorMessage = "Vat must contain 9 digits.")] string vat)
        {
            if (User.FindFirst("vat")?.Value != vat && User.FindFirst("userType")?.Value != UserType.Admin.ToString())
            {
                return Forbid();
            }

            var response = await _propertyItemService.FindByOwner(vat);
            return response;
        }

        // GET
        [HttpGet("{vat}/Repairs")]
        [Authorize]
        public async Task<ActionResult<List<RepairResponseDto>>> GetRepairsOfOwner([RegularExpression(@"^\d{9}$", ErrorMessage = "Vat must contain 9 digits.")] string vat)
        {
            if (User.FindFirst("vat")?.Value != vat && User.FindFirst("userType")?.Value != UserType.Admin.ToString())
            {
                return Forbid();
            }

            var response = await _repairService.FindByOwner(vat);
            return response;
        }

        // PUT
        [HttpPut("{vat}")]
        [Authorize]
        public async Task<ActionResult<PropertyOwnerResponseDto>> PutPropertyOwner([RegularExpression(@"^\d{9}$", ErrorMessage = "Vat must contain 9 digits.")] string vat, PropertyOwnerUpdateRequestDto propertyOwnerUpdateRequestDto)
        {
            if (User.FindFirst("vat")?.Value != vat && User.FindFirst("userType")?.Value != UserType.Admin.ToString())
            {
                return Forbid();
            }

            var response = await _propertyOwnerService.Update(vat, propertyOwnerUpdateRequestDto);
            return response;
        }

        // DELETE
        [HttpDelete("{vat}")]
        [Authorize]
        public async Task<IActionResult> DeletePropertyOwner([RegularExpression(@"^\d{9}$", ErrorMessage = "Vat must contain 9 digits.")] string vat, [FromQuery] bool permanent = false)
        {
            if (User.FindFirst("vat")?.Value != vat && User.FindFirst("userType")?.Value != UserType.Admin.ToString())
            {
                return Forbid();
            }

            var response = await (permanent
                ? _propertyOwnerService.Delete(vat)
                : _propertyOwnerService.SoftDelete(vat));
            return response;
        }

        [HttpPost("login")]
        public async Task<ActionResult<PropertyOwnerLoginResponseDto>> Login(PropertyOwnerLoginRequestDto propertyOwnerLoginRequestDto)
        {
            var response = await _propertyOwnerService.ReadByEmailAndPassword(propertyOwnerLoginRequestDto.Email, propertyOwnerLoginRequestDto.Password);
            return response;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Search([FromQuery] PropertyOwnerFilters properyOwnerFilters)
        {
            if (User.FindFirst("userType")?.Value != UserType.Admin.ToString())
            {
                return Forbid();
            }

            var response = await _propertyOwnerService.Search(properyOwnerFilters);
            return response;
        }

        [HttpPost("me/password-change")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(PropertyOwnerPasswordChangeRequestDto passwordChangeRequestDto)
        {
            var response = await _propertyOwnerService.UpdatePassword(User.FindFirst("vat")!.Value, passwordChangeRequestDto);
            return response;
        }
    }
}
