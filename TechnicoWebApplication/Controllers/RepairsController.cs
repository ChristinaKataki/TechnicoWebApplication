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

        public RepairsController(RepairService repairService)
        {
            _repairService = repairService;
        }

        // CREATE
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<RepairResponseDto>> PostRepair(RepairRequestDto repairRequestDto)
        {
            if (User.FindFirst("vat")?.Value != repairRequestDto.Vat && User.FindFirst("userType")?.Value != UserType.Admin.ToString())
            {
                return Forbid();
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
        public async Task<IActionResult> DeleteRepair(long id)
        {
            if (User.FindFirst("userType")?.Value != UserType.Admin.ToString())
            {
                string? vat = await _repairService.GetOwnerOfRepair(id);
                if (vat != null && User.FindFirst("vat")?.Value != vat)
                {
                    return Forbid();
                }
            }

            var response = await _repairService.Delete(id);
            return response;
        }
    }
}
