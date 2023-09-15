using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RWS.Application.Wrappers;
using System.Threading;
using RWS.Application.Interfaces.Repositories;
using AutoMapper;
using RWS.Domain.Entities;
using RWS.Application.ViewModels;

namespace RWS.Application.Features.TrainStations.Commands.Create
{
    public class CreateTrainStationCommand : IRequest<Response<TrainStationViewModel>>
    {
        public string Name { get; set; }
    }

    public class CreateTrainStationCommandHandler : IRequestHandler<CreateTrainStationCommand, Response<TrainStationViewModel>>
    {
        private readonly ITrainStationRepository _repository;
        private readonly IMapper _mapper;

        public CreateTrainStationCommandHandler(ITrainStationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<TrainStationViewModel>> Handle(CreateTrainStationCommand command, CancellationToken cancellationToken)
        {
            var model = _mapper.Map<TrainStation>(command);
            await _repository.AddAsync(model);
            var viewModel = _mapper.Map<TrainStationViewModel>(model);
            return new Response<TrainStationViewModel>(viewModel, "Станция успешно добавлена.");
        }
    }
}
