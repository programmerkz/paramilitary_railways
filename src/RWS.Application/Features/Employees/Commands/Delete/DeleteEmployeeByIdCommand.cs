using RWS.Application.Exceptions;
using RWS.Application.Interfaces.Repositories;
using RWS.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RWS.Application.ViewModels;

namespace RWS.Application.Features.Employees.Commands.Delete
{
    public class DeleteEmployeeByIdCommand : IRequest<Response<IdViewModel<Guid>>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteEmployeeByIdCommandHandler : IRequestHandler<DeleteEmployeeByIdCommand, Response<IdViewModel<Guid>>>
    {
        private readonly IEmployeeRepository _repository;

        public DeleteEmployeeByIdCommandHandler(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<IdViewModel<Guid>>> Handle(DeleteEmployeeByIdCommand command, CancellationToken cancellationToken)
        {
            var model = await _repository.GetByIdAsync(command.Id);
            if (model == null) throw new ApiException($"Сотрудник не найден.");
            await _repository.DeleteAsync(model);
            var idViewModel = new IdViewModel<Guid>() { Id = model.Id };
            return new Response<IdViewModel<Guid>>(idViewModel, "Сотрудник успешно удален.");
        }
    }
}
