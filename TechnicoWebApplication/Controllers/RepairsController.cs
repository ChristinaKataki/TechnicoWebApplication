using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechnicoWebApplication.Dtos;
using TechnicoWebApplication.Enums;
using TechnicoWebApplication.Services;

namespace TechnicoWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepairsController : ControllerBase
    {
        private readonly RepairService _repairService;
        private readonly PropertyItemService _propertyItemService;

        public RepairsController(RepairService repairService, PropertyItemService propertyItemService)
        {
            _repairService = repairService;
            _propertyItemService = propertyItemService;
        }

        // CREATE
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<RepairResponseDto>> PostRepair(RepairRequestDto repairRequestDto)
        {
            if (User.FindFirst("userType")?.Value != UserType.Admin.ToString())
            {
                string? vat = await _propertyItemService.GetOwnerOfItem(repairRequestDto.PropertyItemId);
                if (vat != null && User.FindFirst("vat")?.Value != vat)
                {
                    return Forbid();
                }
            }

            var response = await _repairService.Create(repairRequestDto);
            return response;
        }

        // GET
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<RepairResponseDto>> GetRepair(long id)
        {
            if (User.FindFirst("userType")?.Value != UserType.Admin.ToString())
            {
                string? vat = await _repairService.GetOwnerOfRepair(id);
                if (vat != null && User.FindFirst("vat")?.Value != vat)
                {
                    return Forbid();
                }
            }

            var response = await _repairService.Read(id);
            return response;
        }

        // PUT
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<RepairResponseDto>> PutRepair(long id, RepairRequestDto repairRequestDto)
        {
            if (User.FindFirst("userType")?.Value != UserType.Admin.ToString())
            {
                string? vat = await _repairService.GetOwnerOfRepair(id);
                if (vat != null && User.FindFirst("vat")?.Value != vat)
                {
                    return Forbid();
                }
            }

            var response = await _repairService.Update(id, repairRequestDto);
            return response;
        }

        // DELETE
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteRepair(long id, [FromQuery] bool permanent = false)
        {
            if (User.FindFirst("userType")?.Value != UserType.Admin.ToString())
            {
                string? vat = await _repairService.GetOwnerOfRepair(id);
                if (vat != null && User.FindFirst("vat")?.Value != vat)
                {
                    return Forbid();
                }
            }

            var response = await (permanent
                ? _repairService.Delete(id)
                : _repairService.SoftDelete(id));
            return response;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Search([FromQuery] RepairFilters repairFilters)
        {
            if (User.FindFirst("userType")?.Value != UserType.Admin.ToString())
            {
                repairFilters.Vat = User.FindFirst("vat")?.Value;
            }

            var response = await _repairService.Search(repairFilters);
            return response;
        }
    }
}
