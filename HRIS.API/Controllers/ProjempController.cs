using HRIS.Domain.Interfaces;
using HRIS.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRIS.API.Controllers
{
    [Route("api/projemp")]
    [ApiController]

    public class ProjempController : ControllerBase
    {
        private readonly IProjempService _projempService;

        public ProjempController(IProjempService projempService)
        {
            _projempService = projempService;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,HR Manager,Department Manager,Employee Supervisor")]
        public async Task<IActionResult> GetAllProjemp()
        {
            var projemp = await _projempService.GetAllProjempAsync();
            return Ok(projemp);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator,HR Manager,Department Manager,Employee Supervisor")]
        public async Task<IActionResult> GetProjempById(int id)
        {
            var projemp = await _projempService.GetProjempByIdAsync(id);

            if (projemp == null)
            {
                return NotFound("Projemp not found");
            }

            return Ok(projemp);
        }

        [HttpPost]
        //[Authorize(Roles = "Administrator,Department Manager")]
        [AllowAnonymous]
        public async Task<IActionResult> AddProjemp([FromBody] Projemp projemp)
        {
            if (projemp == null)
            {
                return BadRequest("Invalid projemp data");
            }

            var result = await _projempService.AddProjempAsync(projemp);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator,Department Manager")]
        public async Task<IActionResult> UpdateProjemp(int id, [FromBody] Projemp projemp)
        {
            if (id != projemp.Id)
            {
                return BadRequest("Projemp ID mismatch");
            }

            var result = await _projempService.UpdateProjempAsync(projemp);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator,Department Manager")]
        public async Task<IActionResult> DeleteProjemp(int id)
        {
            var result = await _projempService.DeleteProjempAsync(id);
            if (result == "Projemp not found")
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
