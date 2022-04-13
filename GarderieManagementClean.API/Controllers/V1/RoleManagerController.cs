using GarderieManagementClean.Infrastructure.Identity;
using GarderieManagementClean.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GarderieManagementClean.API.Controllers.V1
{
   // [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleManagerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly ILoggerManager _logger;

        public RoleManagerController(
          ApplicationDbContext context,
          UserManager<ApplicationUser> userManager,
          RoleManager<IdentityRole> roleManager
         )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            //_logger = logger;
        }



        //TODO: Move this to different controller??
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {

            var users = await _userManager.Users.ToListAsync();
            return Ok(users);

        }


        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }

        [HttpGet("GetRoleByName")]
        public async Task<IActionResult> GetRoleByName(string roleName)
        {

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role is null)
            {
                return NotFound();
            }

            return Ok(role);
        }
        [HttpGet("GetRoleById")]

        public async Task<IActionResult> GetRoleById(string roleId)
        {

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
            {
                return NotFound();
            }

            return Ok(role);
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole(string role)
        {


            var roleCreated = await _roleManager.CreateAsync(new IdentityRole(role));
            if (!roleCreated.Succeeded)
            {
                return BadRequest(new { Errors = roleCreated.Errors.Select(err => err.Description) });
            }

         
            return CreatedAtAction(nameof(GetRoleByName), roleCreated);
        }


        [HttpDelete("RemoveRole")]
        public async Task<IActionResult> RemoveRole(string role)
        {
            var existingRole = await _roleManager.FindByNameAsync(role);
            if (existingRole == null)
            {
                var error = $"Role [{role}] doesn't exist.";
                return BadRequest(new { error = error });
            }
            var result = await _roleManager.DeleteAsync(existingRole);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(err => err.Description);
                ////_logger.LogInfo($"{errors}");
                return BadRequest(new { Error = errors });
            }

            var sucess = $"{role} successfully deleted";
            //_logger.LogInfo(sucess);
            return NoContent();
        }


        [HttpGet("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            //Check if User exists
            var existingUser = await _userManager.FindByIdAsync(userId);
            if (existingUser == null)
            {
                //_logger.LogInfo($"User [{userId}] doesn't exist.");
                return BadRequest(new { Error = $"User [{userId}] doesn't exist." });
            }

            var userRoles = await _userManager.GetRolesAsync(existingUser);
            //_logger.LogInfo($"Successfuly retrieved User [{userId}] roles");
            return Ok(userRoles);
        }


        [HttpPost("AssignRoleToUser")]
        public async Task<IActionResult> AssignRoleToUser(string userId, string role)
        {
            //Check if User exists
            var existingUser = await _userManager.FindByIdAsync(userId);
            if (existingUser == null)
            {
                //_logger.LogInfo($"User [{userId}] doesn't exist.");
                return BadRequest(new { Error = $"User [{userId}] doesn't exist." });
            }

            //Check if Role exists
            var existingRole = await _roleManager.FindByNameAsync(role);
            if (existingRole == null)
            {
                //_logger.LogInfo($"Role [{role}] doesn't exist.");
                return BadRequest(new { Error = $"Role [{role}] doesn't exist." });
            }

            var result = await _userManager.AddToRoleAsync(existingUser, role);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(err => err.Description);
                //_logger.LogInfo($"{errors}");
                return BadRequest(new { Error = errors });
            }

            var sucess = $"Role {role} assigned to user {userId} successfuly";
            //_logger.LogInfo(sucess);
            return Ok(new { Success = sucess });
        }


        [HttpPost("RemoveRoleFromUser")]
        public async Task<IActionResult> RemoveRoleFromUser(string userId, string role)
        {
            //Check if User exists
            var existingUser = await _userManager.FindByIdAsync(userId);
            if (existingUser == null)
            {
                //_logger.LogInfo($"User [{userId}] doesn't exist.");
                return BadRequest(new { Error = $"User [{userId}] doesn't exist." });
            }

            //Check if Role exists
            var existingRole = await _roleManager.FindByNameAsync(role);
            if (existingRole == null)
            {
                //_logger.LogInfo($"Role [{role}] doesn't exist.");
                return BadRequest(new { Error = $"Role [{role}] doesn't exist." });
            }

            var result = await _userManager.RemoveFromRoleAsync(existingUser, role);


            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(err => err.Description);
                //_logger.LogInfo($"{errors}");
                return BadRequest(new { Error = errors });
            }

            var sucess = $"Role {role} removed from user {userId} successfuly";
            //_logger.LogInfo(sucess);
            return Ok(new { Success = sucess });
        }

    }
}
