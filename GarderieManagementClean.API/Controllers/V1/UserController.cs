﻿using AutoMapper;
using Contracts.Dtos;
using GarderieManagementClean.API.Extensions;
using GarderieManagementClean.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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


        [Authorize()]
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
                result.Data = _mapper.Map<ApplicationUserDTO>(result.Data);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }


        [Authorize()]
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
                result.Data = _mapper.Map<ApplicationUserDTO>(result.Data);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
    }
}
