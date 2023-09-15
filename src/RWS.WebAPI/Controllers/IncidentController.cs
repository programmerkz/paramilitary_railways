using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using RWS.WebAPI.Controllers.Common;
using RWS.Application.Features.Incidents.Commands.Create;
using RWS.Application.Features.Incidents.Queries.GetAll;
using RWS.Application.Features.Incidents.Queries.GetById;
using RWS.Application.Features.Incidents.Commands.Update;
using RWS.Application.Features.Incidents.Commands.Delete;
using RWS.Application.Features.Incidents.Commands.Send;
using RWS.Application.Features.Incidents.Queries.GetAllFiltered;
using Microsoft.AspNetCore.Authorization;
using RWS.Application.Helpers.Contracts;

namespace RWS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = RwsRoles.ADMIN_ROLE_NAME + "," + RwsRoles.USER_ROLE_NAME)]
    public class IncidentController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllIncidentsQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("Filtered/")]
        public async Task<IActionResult> GetFiltered([FromQuery] GetAllFilteredIncidentsParameter filter)
        {
            return Ok(await Mediator.Send(new GetAllFilteredIncidentsQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber, IsSent = filter.IsSent }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await Mediator.Send(new GetIncidentByIdQuery { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateIncidentCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UpdateIncidentCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("Reveal/{id}")]
        public async Task<IActionResult> Reveal(Guid id, SendIncidentCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await Mediator.Send(new DeleteIncidentByIdCommand { Id = id }));
        }
    }
}
