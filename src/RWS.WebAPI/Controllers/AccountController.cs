using RWS.Application.Exceptions;
using RWS.Application.Helpers.Contracts;
using RWS.Application.Interfaces;
using RWS.Application.Requests;
using RWS.Application.Responses;
using RWS.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RWS.WebAPI.Controllers.Common;
using RWS.Application.ViewModels;
using Microsoft.Net.Http.Headers;

namespace RWS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : BaseApiController
    {
        private readonly IAuthLogic _logic;

        public AccountController(IAuthLogic logic)
        {
            _logic = logic;
        }

        /// <summary>
        /// Sign-in
        /// </summary>
        /// <param name="model">Login and Password</param>
        /// <response code="200">Successfully</response>
        /// <response code="403">Wrong login or password</response>
        [ProducesResponseType(typeof(Response<AuthResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status403Forbidden)]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInRequest model)
        {
            return Ok(await _logic.SignInAsync(model));
        }



        /// <summary>
        /// Sign-up
        /// </summary>
        /// <param name="model">New User model including Login, Password and Profile data</param>
        /// <response code="200">Successfully</response>
        /// <response code="400">Password or Role doesn'r meet the requirements</response>
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [HttpPost("SignUp")]
        [Authorize(Roles = RwsRoles.ADMIN_ROLE_NAME)]
        public async Task<IActionResult> SignUp(SignUpRequest model)
        {
            return Ok(await _logic.SignUpAsync(model, RwsRoles.ADMIN_ROLE_NAME));
        }



        /// <summary>
        /// Refresh token
        /// </summary>
        /// <param name="request">Request containing Token and RefreshToken</param>
        /// <response code="200">Successfully. New Token and Refresh token are generated.</response>
        /// <response code="400">One or more validation failures have occurred</response>
        [ProducesResponseType(typeof(Response<AuthResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            return Ok(await _logic.RefreshTokenAsync(request));
        }


        /// <summary>
        /// Get a user's Profile by the userId parameter
        /// </summary>
        /// <response code="200">Successfully</response>
        /// <response code="400">User isn't found</response>
        [ProducesResponseType(typeof(Response<ProfileViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = RwsRoles.ADMIN_ROLE_NAME)]
        [HttpGet("GetProfile/{userId}")]
        public async Task<IActionResult> GetProfile(string userId)
        {
            return Ok(await _logic.GetProfileAsync(userId));
        }

        /// <summary>
        /// Get current user's Profile
        /// </summary>
        /// <response code="200">Successfully</response>
        /// <response code="400">User isn't found</response>
        [ProducesResponseType(typeof(Response<ProfileViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = RwsRoles.USER_ROLE_NAME)]
        [HttpGet("GetProfile")]
        public async Task<IActionResult> GetProfile()
        {
            return Ok(await _logic.GetProfileByTokenAsync(Request.Headers[HeaderNames.Authorization]));
        }



        /// <summary>
        /// Get all user profiles
        /// </summary>
        /// <response code="200">Successfully</response>
        [ProducesResponseType(typeof(Response<List<ProfileViewModel>>), StatusCodes.Status200OK)]
        [Authorize(Roles = RwsRoles.ADMIN_ROLE_NAME)]
        [HttpGet("GetAllProfiles")]
        public async Task<IActionResult> GetAllProfiles()
        {
            return Ok(await _logic.GetAllProfilesAsync());
        }


    }
}
