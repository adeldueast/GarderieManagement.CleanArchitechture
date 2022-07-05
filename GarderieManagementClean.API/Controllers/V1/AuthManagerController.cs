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
using System.Security.Claims;
using System.Threading.Tasks;

namespace GarderieManagementClean.API.Controllers.V1
{
    // [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class AuthManagerController : ControllerBase
    {

        private readonly IIdentityService _identityService;
        public AuthManagerController(IIdentityService identityService)
        {
            _identityService = identityService;
        }


        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegistrationRequest request)
        {
            try
            {



                var authResult = await _identityService.RegisterOwnerAsync(request);

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

        [AllowAnonymous]
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

                var authResult = await _identityService.AuthenticateAsync(request);

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



    
        //TODO: MOVE INVITE STAFF AND INVITE TUTOR IN USER REPOSITORY INSTEAD OF AUTH REPO
        [HttpPost(ApiRoutes.Identity.InviteStaff)]
        public async Task<IActionResult> InviteUser([FromBody] UserInviteUserRequest inviteUserRequest)
        {
            try
            {

                var userId = HttpContext.GetUserId();
                var result = await _identityService.InviteUser(userId, inviteUserRequest);


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


       
        [HttpPost(ApiRoutes.Identity.InviteTutor)]
        
        public async Task<IActionResult> InviteTutor([FromBody] UserInviteTutorRequest inviteTutorRequest)
        {
            try
            {

                var userId = HttpContext.GetUserId();
                var result = await _identityService.InviteTutor(userId, inviteTutorRequest);


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


        #region REFRESH TOKENS AND CONFIRM EMAIL
        //REFRESH TOKEN
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost(ApiRoutes.Identity.RefreshToken)]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] UserRefreshTokenRequest tokenRequest)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(x => x.Errors.Select(err => err.ErrorMessage));
                    // _logger.LogInfo($"Failed to refreshToken, {errors} ");
                    return BadRequest(new ErrorResponse(errors));


                }
                var authResult = await _identityService.RefreshTokenAsync(tokenRequest);

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
            var deletedTokens = await _identityService.RevokeTokensAsync(userId);

            return Ok(deletedTokens);

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet(ApiRoutes.Identity.ConfirmEmail)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] UserValidateEmailRequest validateEmailRequest)
        {
            try
            {
                var result = await _identityService.ConfirmEmailOrInvitationAsync(validateEmailRequest.userId, validateEmailRequest.ConfirmEmailToken);

                if (!result.Success)
                {
                    return BadRequest(result);
                }
                //TODO: Redirect to client complete-registration page & include userId in 

                return Ok(result);
            }
            catch (Exception e)
            {

                return StatusCode(500, new Result<object>() { Errors = new string[] { $"{e.Message}" } });
            }
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost(ApiRoutes.Identity.CompleteRegister)]
        public async Task<IActionResult> CompleteRegistration([FromBody] UserCompleteRegistrationRequest completeRegistrationRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(x => x.Errors.Select(err => err.ErrorMessage));
                    // _logger.LogInfo($"Failed to refreshToken, {errors} ");
                    return BadRequest(new ErrorResponse(errors));

                }


                var result = await _identityService.CompleteRegistration(completeRegistrationRequest);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception e)
            {

                //  _logger.LogError($"RefreshTokenAsync : {e.Message}, {e.InnerException.Message}");
                return StatusCode(500, new Result<object>() { Errors = new string[] { $"{e.Message}" } });
            }
        }
        #endregion
    }
}
