using Contracts.Dtos.Request;
using GarderieManagementClean.API.Extensions;
using GarderieManagementClean.Application.Interfaces.Services;
using GarderieManagementClean.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GarderieManagementClean.API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnfantController : ControllerBase
    {
        private readonly IEnfantService _enfantService;
        public EnfantController(IEnfantService enfantService)
        {
            _enfantService = enfantService;
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "owner,admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> createEnfant([FromBody] EnfantCreateRequest enfantCreateRequest)
        {
            var userId = HttpContext.GetUserId();

          

            var result = await _enfantService.createEnfant(userId, enfantCreateRequest);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
