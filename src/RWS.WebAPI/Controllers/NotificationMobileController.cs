using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RWS.Application.Features.NotificationMobile.Command.Create;
using RWS.Application.Features.NotificationMobile.Command.Delete;
using RWS.WebAPI.Controllers.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RWS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationMobileController : BaseApiController
    {
        /// <summary>
        /// Create push token
        /// </summary>
        /// <response code="200">Successful operation</response>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> CreatePushToken(CreateNotificationTokenCommand command)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "usr_id").Value;
            command.SetUserId(Guid.Parse(userId));
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete push token
        /// </summary>
        /// <response code="200">Successful operation</response>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [HttpDelete]
        public async Task<IActionResult> DeletePushToken()
        {
            var command = new DeleteNotificationTokenCommand();
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "usr_id").Value;
            command.SetUserId(Guid.Parse(userId));
            return Ok(await Mediator.Send(command));
        }
    }
}
