﻿using AutoMapper;
using Contracts.Dtos.Request;
using Contracts.Dtos.Response;
using GarderieManagementClean.API.Extensions;
using GarderieManagementClean.API.HubConfig;
using GarderieManagementClean.Application.Interfaces.Services;
using GarderieManagementClean.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarderieManagementClean.API.Controllers.V1
{

    [Authorize]
    [ApiController]
    public class EnfantController : ControllerBase
    {
        private readonly IEnfantService _enfantService;
        private readonly IMapper _mapper;

        private readonly IHubContext<ChildrenHub> _hubContext;
        public EnfantController(IEnfantService enfantService, IMapper mapper, IHubContext<ChildrenHub> hubContext)
        {
            _enfantService = enfantService;
            _mapper = mapper;

            _hubContext = hubContext;
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

                await _hubContext.Clients.Group(HttpContext.GetUserGarderieId()).SendAsync("childUpdate", result.Data);
                return CreatedAtAction(nameof(getEnfantById), new { enfantId = (result.Data as EnfantResponse).Id }, result);


            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }

        [Authorize(Roles = "owner,admin,employee")]
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

                //   result.Data = _mapper.Map<List<EnfantSummariesResponse>>(result.Data as List<Enfant>);
                // result.Data = _mapper.Map(da,result.Data as List<Enfant>);

                return Ok(result);
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }


        [Authorize(Roles = "tutor")]

        [HttpGet(ApiRoutes.Enfant.GetAllTutorsChilds)]
        public async Task<IActionResult> getAllTutorsChilds()
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var result = await _enfantService.getAllTutorsEnfants(userId);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                //   result.Data = _mapper.Map<List<EnfantSummariesResponse>>(result.Data as List<Enfant>);
                // result.Data = _mapper.Map(da,result.Data as List<Enfant>);

                return Ok(result);
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }

        [Authorize(Roles = "owner,admin,employee")]
        [HttpGet(ApiRoutes.Enfant.GetAllGrouped)]
        public async Task<IActionResult> getAllEnfantsGroupedByGroup()
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var result = await _enfantService.getAllEnfantsGroupedByGroup(userId);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                //   result.Data = _mapper.Map<List<EnfantSummariesResponse>>(result.Data as List<Enfant>);
                // result.Data = _mapper.Map(da,result.Data as List<Enfant>);

                return Ok(result);
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }

        [Authorize(Roles = "owner,admin,employee,tutor")]
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



        [Authorize(Roles = "owner,admin")]
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


                //NOTIFY ALL SUBSCRIBED CLIENTS 
                await _hubContext.Clients.Group(HttpContext.GetUserGarderieId()).SendAsync("childUpdate", result.Data);
                    
                return Ok(result);
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }


        [Authorize(Roles = "owner,admin")]
        [HttpPost(ApiRoutes.Enfant.Delete)]
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
                await _hubContext.Clients.Group(HttpContext.GetUserGarderieId()).SendAsync("childUpdate", result.Data);
                return Ok(result);
            }
            catch (Exception e)
            {
                //{"The DELETE statement conflicted with the REFERENCE constraint \"FK_Photos_Enfants_EnfantId\".
                //The conflict occurred in database \"Garderie.Management.Clean\", table \"dbo.Photos\", column 'EnfantId'.\r\nThe statement has been terminated."}
                return StatusCode(500, e.Message);
            }
        }



        [Authorize(Roles = "owner,admin")]
        [HttpPost(ApiRoutes.Enfant.AssignTutorToEnfant)]
        public async Task<IActionResult> assignTutorToEnfant(EnfantAssignTutorRequest enfantAssignTutorRequest)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var result = await _enfantService.assignTutorToEnfant(userId, enfantAssignTutorRequest);

                if (!result.Success)
                {
                    return BadRequest(result);
                }
                await _hubContext.Clients.Group(HttpContext.GetUserGarderieId()).SendAsync("childUpdate", result.Data);
                return Ok(result);
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }
    }
}
