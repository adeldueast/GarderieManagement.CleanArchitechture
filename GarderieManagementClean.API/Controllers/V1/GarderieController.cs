using AutoMapper;
using Contracts.Request;
using Contracts.Response;
using GarderieManagementClean.API.Extensions;
using GarderieManagementClean.Application.Interfaces;
using GarderieManagementClean.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace GarderieManagementClean.API.Controllers.V1
{

    [ApiController]
    public class GarderieController : ControllerBase
    {

        private readonly IGarderieService _garderieService;
        private readonly IMapper _mapper;
        public GarderieController(IGarderieService garderieService, IMapper mapper)
        {
            _garderieService = garderieService;
            _mapper = mapper;
        }




        [Authorize]
        [HttpGet(ApiRoutes.Garderie.Get)]
        public async Task<IActionResult> GetGarderieInfo()
        {
            var userId = HttpContext.GetUserId();


            var result = await _garderieService.getGarderie(userId);

            //if (garderieDomain == null) return NotFound();

            //var garderieReponse = _mapper.Map<GarderieResponse>(garderieDomain);
            if (result.Success)
            {
                result.Data = _mapper.Map<GarderieResponse>(result.Data);
                return Ok(result);
            }
            return BadRequest(result);
        }




        [Authorize(Roles = "owner")]
        [HttpPost(ApiRoutes.Garderie.Create)]
        public async Task<IActionResult> CreateGarderie([FromBody] GarderieCreateRequest garderieRequest)
        {
            var userId = HttpContext.GetUserId();

            var mappedGarderieRequest = _mapper.Map<Garderie>(garderieRequest);

            var result = await _garderieService.createGarderie(userId, mappedGarderieRequest);

            //if (garderieDomain == null) return NotFound();

            //var garderieReponse = _mapper.Map<GarderieResponse>(garderieDomain);
            if (result.Success)
            {
                result.Data = _mapper.Map<GarderieResponse>(result.Data);
                return Ok(result);
            }
            return BadRequest(result);
        }




        [Authorize( Roles = "owner")]
        [HttpPut(ApiRoutes.Garderie.Update)]
        public async Task<IActionResult> UpdateGarderie([FromBody] GarderieCreateRequest updatedGarderieRequest)
        {
            //retrieve loggedin user Id
            var userId = HttpContext.GetUserId();

            //map the DTO  to domain object
            var mappedUpdatedGarderieRequest = _mapper.Map<Garderie>(updatedGarderieRequest);

            var result = await _garderieService.updateGarderie(userId, mappedUpdatedGarderieRequest);

            //if (updatedGarderie == null) return NotFound();
            //var garderieResponse = _mapper.Map<GarderieResponse>(updatedGarderie);

            if (result.Success)
            {
                result.Data = _mapper.Map<GarderieResponse>(result.Data);
                return Ok(result);
            }
            return BadRequest(result);
        }




        [Authorize( Roles = "owner")]
        [HttpPost(ApiRoutes.Garderie.Delete)]
        public async Task<IActionResult> DeleteGarderie()
        {
            var userId = HttpContext.GetUserId();

            var result = await _garderieService.deleteGarderie(userId);

            //if (garderieDomain == null) return NotFound();

            //var garderieReponse = _mapper.Map<GarderieResponse>(garderieDomain);

            if (result.Success)
            {
                result.Data = _mapper.Map<GarderieResponse>(result.Data);
                return Ok(result);
            }
            return BadRequest(result);
        }


    }
}
