using Contracts.Dtos.Request;
using Contracts.Request;
using Contracts.Response;
using GarderieManagementClean.API.Extensions;
using GarderieManagementClean.Application.Interfaces;
using GarderieManagementClean.Application.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GarderieManagementClean.API.Controllers.V1
{
    // [Route("api/[controller]")]
    [ApiController]
    public class AuthManagerController : ControllerBase
    {


        private readonly IIdentityService _identityService;
        public AuthManagerController(IIdentityService identityService)
        {

            _identityService = identityService;
        }


        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegistrationRequest request)
        {
            try
            {


                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(x => x.Errors.Select(err => err.ErrorMessage));
                    //_logger.LogInfo($"Failed to register, {errors} ");
                    return BadRequest(new Result<object>() { Errors = errors });
                }



                var authResult = await _identityService.RegisterOwnerAsync(request.Email, request.Password);

                if (!authResult.Success)
                {
                    //_logger.LogInfo("Failed to register");
                    return BadRequest(authResult);
                }

                // _logger.LogInfo("Successfully registered");
                return Ok(authResult);

            }
            catch (Exception e)
            {

                //_logger.LogError($"AuthenticateAsync : {e.Message}");
                return StatusCode(500, new Result<object>() { Errors = new string[] { $"{e.Message}" } });
            }
        }


        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> AuthenticateAsync([FromBody] UserLoginRequest request)
        {

            try
            {

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(x => x.Errors.Select(err => err.ErrorMessage));
                    // _logger.LogInfo($"Failed to login, {errors} ");
                    return BadRequest(new ErrorResponse(errors));
                }

                var authResult = await _identityService.AuthenticateAsync(request.Email, request.Password);

                if (!authResult.Success)
                {
                    // _logger.LogInfo("Failed to log in");
                    return BadRequest(authResult);
                }

                // _logger.LogInfo("Successfully logged in");
                return Ok(authResult);
            }
            catch (Exception e)
            {

                //_logger.LogError($"AuthenticateAsync : {e.Message}");
                return StatusCode(500, new Result<object>() { Errors = new string[] { $"{e.Message}" } });

            }

        }


        [HttpGet(ApiRoutes.Identity.ConfirmEmail)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ValidateEmailRequest validateEmailRequest)
        {
            try
            {
                var result = await _identityService.ConfirmEmailOrInvitationAsync(validateEmailRequest.userId, validateEmailRequest.Token);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception e)
            {

                return StatusCode(500, new Result<object>() { Errors = new string[] { $"{e.Message}" } });
            }
        }


        [HttpPost(ApiRoutes.Identity.InviteUser)]
        public async Task<IActionResult> InviteUser([FromBody] InviteUserRequest inviteUserRequest)
        {
            try
            {
                var result = await _identityService.InviteUser(inviteUserRequest.userEmail, inviteUserRequest.role);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception e)
            {

                return StatusCode(500, new Result<object>() { Errors = new string[] { $"{e.Message}" } });
            }
        }


        [HttpPost(ApiRoutes.Identity.RefreshToken)]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest tokenRequest)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(x => x.Errors.Select(err => err.ErrorMessage));
                    // _logger.LogInfo($"Failed to refreshToken, {errors} ");
                    return BadRequest(new ErrorResponse(errors));


                }
                var authResult = await _identityService.RefreshTokenAsync(tokenRequest.AccessToken, tokenRequest.RefreshToken);

                if (!authResult.Success)
                {
                    return BadRequest(authResult);
                }

                return Ok(authResult);
            }
            catch (Exception e)
            {


                //  _logger.LogError($"RefreshTokenAsync : {e.Message}, {e.InnerException.Message}");
                return StatusCode(500, new Result<object>() { Errors = new string[] { $"{e.Message}" } });

            }

        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete(ApiRoutes.Identity.RevokeTokens)]
        public async Task<IActionResult> RevokeAllTokens()
        {
            var userId = HttpContext.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { Errors = new[] { $"User with id '{userId}' not found" } });
            }
            var deletedTokens = await _identityService.RevokeTokensAsync(HttpContext.GetUserId());

            return Ok(deletedTokens);

        }

    }
}
