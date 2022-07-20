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
using System.Threading.Tasks;

namespace GarderieManagementClean.API.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "owner,admin,employee")]

    [ApiController]
    public class JournalController : ControllerBase
    {
        private readonly IJournalDeBordService _journalService;
        private readonly IMapper _mapper;
        public JournalController(IJournalDeBordService journalService, IMapper mapper)
        {
            _journalService = journalService;
            _mapper = mapper;
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


                result.Data = _mapper.Map<JournalResponse>(result.Data);
                return Ok(result);
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }



        [Authorize(Roles = "owner,admin,employee")]
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
    }
}
