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

namespace RWS.Application.Features.IncidentTypes.Commands.Delete
{
    public class DeleteIncidentTypeByIdCommand : IRequest<Response<IdViewModel<Guid>>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteIncidentTypeByIdCommandHandler : IRequestHandler<DeleteIncidentTypeByIdCommand, Response<IdViewModel<Guid>>>
    {
        private readonly IIncidentTypeRepository _repository;

        public DeleteIncidentTypeByIdCommandHandler(IIncidentTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<IdViewModel<Guid>>> Handle(DeleteIncidentTypeByIdCommand command, CancellationToken cancellationToken)
        {
            var model = await _repository.GetByIdAsync(command.Id);
            if (model == null) throw new ApiException($"Тип инцидента не найден.");
            await _repository.DeleteAsync(model);
            var idViewModel = new IdViewModel<Guid>() { Id = model.Id};
            return new Response<IdViewModel<Guid>>(idViewModel, "Тип инцидента успешно удален.");
        }
    }
}
