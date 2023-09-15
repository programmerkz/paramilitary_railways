using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using RWS.WebAPI.Controllers.Common;
using RWS.Application.Features.Employees.Commands.Create;
using RWS.Application.Features.Employees.Queries.GetAll;
using RWS.Application.Features.Employees.Queries.GetById;
using RWS.Application.Features.Employees.Commands.Update;
using RWS.Application.Features.Employees.Commands.Delete;
using Microsoft.AspNetCore.Authorization;
using RWS.Application.Helpers.Contracts;

namespace RWS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = RwsRoles.ADMIN_ROLE_NAME + "," + RwsRoles.USER_ROLE_NAME)]
    public class EmployeeController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllEmployeesParameter filter)
        {
            return Ok(await Mediator.Send(new GetAllEmployeesQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await Mediator.Send(new GetEmployeeByIdQuery { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateEmployeeCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UpdateEmployeeCommand command)
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
            return Ok(await Mediator.Send(new DeleteEmployeeByIdCommand { Id = id }));
        }
    }
}
