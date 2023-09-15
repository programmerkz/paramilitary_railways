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

namespace RWS.Application.Features.TrainStations.Commands.Delete
{
    public class DeleteTrainStationByIdCommand : IRequest<Response<IdViewModel<Guid>>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteTrainStationByIdCommandHandler : IRequestHandler<DeleteTrainStationByIdCommand, Response<IdViewModel<Guid>>>
    {
        private readonly ITrainStationRepository _repository;

        public DeleteTrainStationByIdCommandHandler(ITrainStationRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<IdViewModel<Guid>>> Handle(DeleteTrainStationByIdCommand command, CancellationToken cancellationToken)
        {
            var model = await _repository.GetByIdAsync(command.Id);
            if (model == null) throw new ApiException($"Станция не найдена.");
            await _repository.DeleteAsync(model);
            var idViewModel = new IdViewModel<Guid>() { Id = model.Id };
            return new Response<IdViewModel<Guid>>(idViewModel, "Станция успешно удалена.");
        }
    }
}
