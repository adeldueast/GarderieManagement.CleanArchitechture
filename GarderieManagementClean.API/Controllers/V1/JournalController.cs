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
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "owner,admin,employee")]

    [ApiController]
    public class JournalController : ControllerBase
    {
        private readonly IJournalDeBordService _journalService;
        private readonly IMapper _mapper;
        private readonly IHubContext<ChildrenHub> _hubContext;

        public JournalController(IJournalDeBordService journalService, IMapper mapper, IHubContext<ChildrenHub> hubContext)
        {
            _journalService = journalService;
            _mapper = mapper;
            _hubContext = hubContext;
        }


        [Authorize(Roles = "owner,admin,employee")]
        [HttpPost(ApiRoutes.Journal.Create)]
        public async Task<IActionResult> createJournal([FromRoute] int enfantId, [FromBody] JournalCreateRequest journalCreateRequest)
        {
            try
            {
                var userId = HttpContext.GetUserId();

                var journal = _mapper.Map<JournalDeBord>(journalCreateRequest);
                journal.EnfantId = enfantId;

                var result = await _journalService.createJournal(userId, journal);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                var updatedJournal = (result.Data as JournalDeBord);
                var guardiansToNofity = updatedJournal.Enfant.Tutors.Select(te => te.ApplicationUser.Id).ToArray();
                await _hubContext.Clients.Users(guardiansToNofity).SendAsync("newNotification", $"new notification avaible");
                result.Data = _mapper.Map<JournalResponse>(result.Data);
                return Ok(result);
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }


        [Authorize(Roles = "owner,admin,employee")]
        [HttpPost(ApiRoutes.Journal.CreateGrouped)]
        public async Task<IActionResult> createGroupedJournals([FromBody] JournalGroupedCreateRequest journalGroupedCreateRequest)
        {
            try
            {
                var userId = HttpContext.GetUserId();

                var result = await _journalService.createGroupedJournals(userId, journalGroupedCreateRequest);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                var tutors = result.Data as HashSet<string>;
                await _hubContext.Clients.Users(tutors.ToArray()).SendAsync("newNotification", $"new notification avaible");

                return Ok(result);
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }


        [Authorize(Roles = "owner,admin,employee")]
        [HttpPost(ApiRoutes.Journal.Update)]
        public async Task<IActionResult> updateJournal([FromRoute] int enfantId, [FromBody] JournalUpdateRequest journalUpdateRequest)
        {
            try
            {
                var userId = HttpContext.GetUserId();

                var journal = _mapper.Map<JournalDeBord>(journalUpdateRequest);
                journal.EnfantId = enfantId;
                var result = await _journalService.updateJournal(userId, journal);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

               

                var updatedJournal = (result.Data as JournalDeBord);
                var guardiansToNofity = updatedJournal.Enfant.Tutors.Select(te => te.ApplicationUser.Id).ToArray();

                await _hubContext.Clients.Users(guardiansToNofity).SendAsync("newNotification", $"new notification avaible");


                result.Data = _mapper.Map<JournalResponse>(result.Data);

                return Ok(result);
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }



        //TODO: added tutor as authorize, so securise the request in the repo if requester is a tutor, check if he has access to child journal

        [Authorize(Roles = "owner,admin,employee,tutor")]
        [HttpGet(ApiRoutes.Journal.Get)]
        public async Task<IActionResult> getTodayChildsJournal([FromRoute] int enfantId)
        {
            try
            {
                var userId = HttpContext.GetUserId();


                var result = await _journalService.getTodayChildsJournal(userId, enfantId);

                if (!result.Success)
                {
                    return BadRequest(result);
                }


                result.Data = _mapper.Map<JournalResponse>(result.Data);
                return Ok(result);
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }

        //TODO: added tutor as authorize, so securise the request in the repo if requester is a tutor, check if he has access to child journal

        [Authorize(Roles = "owner,admin,employee,tutor")]
        [HttpGet(ApiRoutes.Journal.GetById)]
        public async Task<IActionResult> getJournalById([FromRoute] int journalId)
        {
            try
            {
                var userId = HttpContext.GetUserId();


                var result = await _journalService.getJournalById(userId, journalId);

                if (!result.Success)
                {
                    return BadRequest(result);
                }


                result.Data = _mapper.Map<JournalResponse>(result.Data);
                return Ok(result);
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }
    }
}
