using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicoWebApplication.Context;
using TechnicoWebApplication.Dtos;
using TechnicoWebApplication.Services;

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
        public async Task<ActionResult<PropertyOwnerResponseDto>> PostPropertyOwner(PropertyOwnerRequestDto propertyOwnerRequestDto)
        {
            var response = await _propertyOwnerService.Create(propertyOwnerRequestDto);
            return response;
        }

        // GET
        [HttpGet("{vat}")]
        public async Task<ActionResult<PropertyOwnerResponseDto>> GetPropertyOwner(string vat)
        {
            var response = await _propertyOwnerService.Read(vat);
            return response;
        }

        // GET
        [HttpGet("{vat}/PropertyItems")]
        public async Task<ActionResult<List<PropertyItemResponseDto>>> GetItemsOfOwner(string vat)
        {
            var response = await _propertyItemService.FindByOwner(vat);
            return response;
        }

        // GET
        [HttpGet("{vat}/Repairs")]
        public async Task<ActionResult<List<RepairResponseDto>>> GetRepairsOfOwner(string vat)
        {
            var response = await _repairService.FindByOwner(vat);
            return response;
        }

        // PUT
        [HttpPut("{vat}")]
        public async Task<ActionResult<PropertyOwnerResponseDto>> PutPropertyOwner(string vat, PropertyOwnerRequestDto propertyOwnerRequestDto)
        {
            var response = await _propertyOwnerService.Update(vat, propertyOwnerRequestDto);
            return response;
        }

        // DELETE
        [HttpDelete("{vat}")]
        public async Task<IActionResult> DeletePropertyOwner(string vat)
        {
            var response = await _propertyOwnerService.Delete(vat);
            return response;
        }


    }
}
