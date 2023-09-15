using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using RWS.WebAPI.Controllers.Common;
using RWS.Application.Features.TrainStations.Commands.Create;
using RWS.Application.Features.TrainStations.Queries.GetAll;
using RWS.Application.Features.TrainStations.Queries.GetById;
using RWS.Application.Features.TrainStations.Commands.Update;
using RWS.Application.Features.TrainStations.Commands.Delete;
using System.Collections.Generic;
using RWS.Application.Helpers.Contracts;
using Microsoft.AspNetCore.Authorization;
using RWS.Application.Features.TrainStations.Queries.GetByName;

namespace RWS.WebAPI.Controllers
{
    [ApiController]
    [Authorize(Roles = RwsRoles.ADMIN_ROLE_NAME + "," + RwsRoles.USER_ROLE_NAME)]
    [Route("api/[controller]")]
    public class TrainStationController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllTrainStationsParameter filter)
        {
            return Ok(await Mediator.Send(new GetAllTrainStationsQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await Mediator.Send(new GetTrainStationByIdQuery { Id = id }));
        }

        [HttpGet("Search/ByName")]
        public async Task<IActionResult> GetByName([FromQuery] GetTrainStationsByNameParameter filter)
        {
            return Ok(await Mediator.Send(new GetTrainStationsByNameQuery { SearchingString = filter.SearchingString, PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateTrainStationCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("PostMultiple")]
        public List<Guid> PostMultiple(IList<CreateTrainStationCommand> command)
        {
            var res = new List<Guid>();

            for (int i = 0; i < command.Count; i++)
                res.Add(Guid.NewGuid());

            return res;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UpdateTrainStationCommand command)
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
            return Ok(await Mediator.Send(new DeleteTrainStationByIdCommand { Id = id }));
        }

        [HttpDelete("DeleteMultiple")]
        public List<Guid> DeleteMultiple(IList<Guid> command)
        {
            var res = new List<Guid>();

            for (int i = 0; i < command.Count; i++)
                res.Add(Guid.NewGuid());

            return res;
        }
    }
}
