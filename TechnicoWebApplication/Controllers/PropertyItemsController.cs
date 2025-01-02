using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechnicoWebApplication.Dtos;
using TechnicoWebApplication.Enums;
using TechnicoWebApplication.Services;

namespace TechnicoWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyItemsController : ControllerBase
    {
        private readonly PropertyItemService _propertyItemService;

        public PropertyItemsController(PropertyItemService propertyItemService)
        {
            _propertyItemService = propertyItemService;
        }

        // CREATE
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PropertyItemResponseDto>> PostPropertyItem(PropertyItemRequestDto propertyItemRequestDto)
        {
            if (User.FindFirst("vat")?.Value != propertyItemRequestDto.Vat && User.FindFirst("userType")?.Value != UserType.Admin.ToString())
            {
                return Forbid();
            }

            var response = await _propertyItemService.Create(propertyItemRequestDto);
            return response;
        }

        // GET
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<PropertyItemResponseDto>> GetPropertyItem(string id)
        {
            if (User.FindFirst("userType")?.Value != UserType.Admin.ToString())
            {
                string? vat = await _propertyItemService.GetOwnerOfItem(id);
                if (vat != null && User.FindFirst("vat")?.Value != vat)
                {
                    return Forbid();
                }
            }

            var response = await _propertyItemService.Read(id);
            return response;
        }

        // PUT
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<PropertyItemResponseDto>> PutPropertyItem(string id, PropertyItemRequestDto propertyItemRequestDto)
        {
            if (User.FindFirst("userType")?.Value != UserType.Admin.ToString())
            {
                string? vat = await _propertyItemService.GetOwnerOfItem(id);
                if (vat != null && User.FindFirst("vat")?.Value != vat)
                {
                    return Forbid();
                }
            }

            var response = await _propertyItemService.Update(id, propertyItemRequestDto);
            return response;
        }

        // DELETE
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePropertyItem(string id)
        {
            if (User.FindFirst("userType")?.Value != UserType.Admin.ToString())
            {
                string? vat = await _propertyItemService.GetOwnerOfItem(id);
                if (vat != null && User.FindFirst("vat")?.Value != vat)
                {
                    return Forbid();
                }
            }

            var response = await _propertyItemService.Delete(id);
            return response;
        }
    }
}
