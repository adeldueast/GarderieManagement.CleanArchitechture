using AutoMapper;
using Contracts.Dtos;
using GarderieManagementClean.API.Extensions;
using GarderieManagementClean.Application.Interfaces.Services;
using GarderieManagementClean.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GarderieManagementClean.API.Controllers.V1
{
    
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }


        [Authorize(Roles = "owner,admin,employee")]
        [HttpGet(ApiRoutes.User.GetAllEmployees)]
        public async Task<IActionResult> getAllEmlpoyees()
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var result = await _userService.getAllEmployee(userId);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                result.Data = _mapper.Map<List<ApplicationUserDTO>>(result.Data as List<ApplicationUser>);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }


        [Authorize(Roles = "owner,admin,employee")]
        [HttpGet(ApiRoutes.User.GetAllEmployeesNoGroup)]
        public async Task<IActionResult> getAllEmlpoyeesNoGroup()
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var result = await _userService.getAllEmployeeWithNoGroup(userId);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                result.Data = _mapper.Map<List<ApplicationUserDTO>>(result.Data as List<ApplicationUser>);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }


        [Authorize(Roles = "owner,admin,employee")]
        [HttpGet(ApiRoutes.User.GetAllTutors)]
        public async Task<IActionResult> getAllTutors()
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var result = await _userService.getAllTutors(userId);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                result.Data = _mapper.Map<List<ApplicationUserDTO>>(result.Data as List<ApplicationUser>);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [Authorize(Roles = "owner,admin,employee")]
        [HttpGet(ApiRoutes.User.GetAllChildsTutors)]
        public async Task<IActionResult> getAllChildsTutors(int enfantId)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var result = await _userService.getAllChildsTutors(userId,enfantId);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                result.Data = _mapper.Map<List<TutorEnfantDTO>>(result.Data as List<TutorEnfant>);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
    }
}
