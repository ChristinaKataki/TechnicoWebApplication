using Microsoft.AspNetCore.Mvc;
using TechnicoWebApplication.Dtos;
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
        public async Task<ActionResult<PropertyItemResponseDto>> PostPropertyItem(PropertyItemRequestDto propertyItemRequestDto)
        {
            var response = await _propertyItemService.Create(propertyItemRequestDto);
            return response;
        }

        // GET
        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyItemResponseDto>> GetPropertyItem(string id)
        {
            var response = await _propertyItemService.Read(id);
            return response;
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<ActionResult<PropertyItemResponseDto>> PutPropertyItem(string id, PropertyItemRequestDto propertyItemRequestDto)
        {
            var response = await _propertyItemService.Update(id, propertyItemRequestDto);
            return response;
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePropertyItem(string id)
        {
            var response = await _propertyItemService.Delete(id);
            return response;
        }
    }
}
