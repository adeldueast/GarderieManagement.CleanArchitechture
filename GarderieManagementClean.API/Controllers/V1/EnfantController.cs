using AutoMapper;
using Contracts.Dtos.Request;
using Contracts.Dtos.Response;
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

    [Authorize(Roles ="owner,admin,employee")]
    [ApiController]
    public class EnfantController : ControllerBase
    {
        private readonly IEnfantService _enfantService;
        private readonly IMapper _mapper;
        public EnfantController(IEnfantService enfantService, IMapper mapper)
        {
            _enfantService = enfantService;
            _mapper = mapper;
        }


        [Authorize(Roles = "owner,admin")]
        [HttpPost(ApiRoutes.Enfant.Create)]
        public async Task<IActionResult> createEnfant([FromBody] EnfantCreateRequest enfantCreateRequest)
        {
            try
            {
                var userId = HttpContext.GetUserId();


                var result = await _enfantService.createEnfant(userId, enfantCreateRequest);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                result.Data = _mapper.Map<EnfantResponse>(result.Data);

                return CreatedAtAction(nameof(getEnfantById), new { enfantId = (result.Data as EnfantResponse).Id }, result);


            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }


        [HttpGet(ApiRoutes.Enfant.GetAll)]
        public async Task<IActionResult> getAllEnfants()
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var result = await _enfantService.getAllEnfants(userId);

                if (!result.Success)
                {
                    return BadRequest(result);
                }


                result.Data = _mapper.Map<List<EnfantResponse>>(result.Data as List<Enfant>);
                return Ok(result);
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }


        [HttpGet(ApiRoutes.Enfant.Get)]
        public async Task<IActionResult> getEnfantById(int enfantId)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var result = await _enfantService.getEnfantById(userId, enfantId);

                if (!result.Success)
                {
                    return BadRequest(result);
                }
                result.Data = _mapper.Map<EnfantResponse>(result.Data);
                return Ok(result);
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }

        [Authorize(Roles ="owner,admin")]
        [HttpPost(ApiRoutes.Enfant.Update)]
        public async Task<IActionResult> updateEnfant([FromBody] EnfantUpdateRequest enfantUpdateRequest)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var result = await _enfantService.updateEnfant(userId, enfantUpdateRequest);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                result.Data = _mapper.Map<EnfantResponse>(result.Data);
                return Ok(result);
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }


        [Authorize(Roles = "owner,admin")]
        [HttpGet(ApiRoutes.Enfant.Delete)]
        public async Task<IActionResult> deleteEnfant(int enfantId)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var result = await _enfantService.deleteEnfant(userId, enfantId);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }
    }
}
