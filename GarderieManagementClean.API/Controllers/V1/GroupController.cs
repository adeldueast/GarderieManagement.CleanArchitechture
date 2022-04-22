using AutoMapper;
using Contracts.Dtos.Request;
using Contracts.Dtos.Response;
using Contracts.Response;
using GarderieManagementClean.API.Extensions;
using GarderieManagementClean.Application.Interfaces;
using GarderieManagementClean.Application.Interfaces.Services;
using GarderieManagementClean.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GarderieManagementClean.API.Controllers.V1
{
    //[Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;

        public GroupController(IGroupService groupService, IMapper mapper)
        {
            _groupService = groupService;
            _mapper = mapper;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "owner,employee,admin")]
        [HttpGet(ApiRoutes.Group.Get)]
        public async Task<IActionResult> getGroupById(int groupId)
        {
            var userId = HttpContext.GetUserId();

            var result = await _groupService.getGroupById(userId, groupId);

            if (result.Success)
            {
                result.Data = _mapper.Map<GroupResponse>(result.Data);
                return Ok(result);
            }
            return BadRequest(result);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "owner,employee,admin")]
        [HttpGet(ApiRoutes.Group.GetAll)]
        public async Task<IActionResult> getAllGroups()
        {
            var userId = HttpContext.GetUserId();

            var result = await _groupService.getAllGroups(userId);
         
            if (result.Success)
            {
                result.Data = _mapper.Map<GroupResponse>(result.Data);
                return Ok(result);
            }
            return BadRequest(result);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "owner,admin")]
        [HttpPost(ApiRoutes.Group.Create)]
        public async Task<IActionResult> createGroup([FromBody] GroupCreateRequest createGroupRequest)
        {
            var userId = HttpContext.GetUserId();

            var group = _mapper.Map<Group>(createGroupRequest);

            var result = await _groupService.createGroup(userId, group);

            if (result.Success)
            {
                result.Data = _mapper.Map<GroupResponse>(result.Data);
                return Ok(result);
            }


            return BadRequest(result);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "owner,admin")]
        [HttpPut(ApiRoutes.Group.Update)]
        public async Task<IActionResult> updateGroup([FromBody] GroupUpdateRequest updatedGroup)
        {
            var userId = HttpContext.GetUserId();

            var group = _mapper.Map<Group>(updatedGroup);

            var result = await _groupService.updateGroup(userId, group);
            if (result.Success)

            {
                result.Data = _mapper.Map<GroupResponse>(result.Data);
                return Ok(result);
            }


            return BadRequest(result);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "owner,admin")]
        [HttpDelete(ApiRoutes.Group.Delete)]
        public async Task<IActionResult> deleteGroup(int groupId)
        {
            var userId = HttpContext.GetUserId();

            var result = await _groupService.deleteGroup(userId, groupId);

            if (result.Success)
            {
                result.Data = _mapper.Map<GroupResponse>(result.Data);
                return Ok(result);
            }


            return BadRequest(result);
        }

    }
}
