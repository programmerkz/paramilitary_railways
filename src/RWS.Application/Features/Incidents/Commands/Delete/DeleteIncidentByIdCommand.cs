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

namespace RWS.Application.Features.Incidents.Commands.Delete
{
    public class DeleteIncidentByIdCommand : IRequest<Response<IdViewModel<Guid>>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteIncidentByIdCommandHandler : IRequestHandler<DeleteIncidentByIdCommand, Response<IdViewModel<Guid>>>
    {
        private readonly IIncidentRepository _repository;

        public DeleteIncidentByIdCommandHandler(IIncidentRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<IdViewModel<Guid>>> Handle(DeleteIncidentByIdCommand command, CancellationToken cancellationToken)
        {
            var model = await _repository.GetByIdAsync(command.Id);
            if (model == null) throw new ApiException($"Инцидент не найден.");
            await _repository.DeleteAsync(model);
            var idViewModel = new IdViewModel<Guid>() { Id = model.Id };
            return new Response<IdViewModel<Guid>>(idViewModel, "Инцидент успешно удален.");
        }
    }
}
