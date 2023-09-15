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
using AutoMapper;

namespace RWS.Application.Features.TrainStations.Commands.Update
{
    public class UpdateTrainStationCommand : IRequest<Response<TrainStationViewModel>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateTrainStationCommandHandler : IRequestHandler<UpdateTrainStationCommand, Response<TrainStationViewModel>>
    {
        private readonly ITrainStationRepository _repository;
        private readonly IMapper _mapper;

        public UpdateTrainStationCommandHandler(ITrainStationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<TrainStationViewModel>> Handle(UpdateTrainStationCommand request, CancellationToken cancellationToken)
        {
            var model = await _repository.GetByIdAsync(request.Id);

            if (model == null)
            {
                throw new ApiException($"Станция не найдена.");
            }
            else
            {
                model.Name = request.Name;
                await _repository.UpdateAsync(model);
                var viewModel = _mapper.Map<TrainStationViewModel>(model);
                return new Response<TrainStationViewModel>(viewModel, "Станция успешно обновлена.");
            }
        }
    }
}
