using Microsoft.AspNetCore.Mvc;
using TechnicoWebApplication.Dtos;
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
        public async Task<ActionResult<RepairResponseDto>> PostRepair(RepairRequestDto repairRequestDto)
        {
            var response = await _repairService.Create(repairRequestDto);
            return response;
        }

        // GET
        [HttpGet("{id}")]
        public async Task<ActionResult<RepairResponseDto>> GetRepair(long id)
        {
            var response = await _repairService.Read(id);
            return response;
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<ActionResult<RepairResponseDto>> PutRepair(long id, RepairRequestDto repairRequestDto)
        {
            var response = await _repairService.Update(id, repairRequestDto);
            return response;
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRepair(long id)
        {
            var response = await _repairService.Delete(id);
            return response;
        }
    }
}
