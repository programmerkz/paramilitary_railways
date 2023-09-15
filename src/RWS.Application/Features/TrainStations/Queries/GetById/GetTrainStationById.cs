using RWS.Application.Exceptions;
using RWS.Application.Interfaces.Repositories;
using RWS.Application.Wrappers;
using RWS.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using RWS.Application.ViewModels;

namespace RWS.Application.Features.TrainStations.Queries.GetById
{
    public class GetTrainStationByIdQuery : IRequest<Response<TrainStationViewModel>>
    {
        public Guid Id { get; set; }
    }

    public class GetTrainStationByIdHandler : IRequestHandler<GetTrainStationByIdQuery, Response<TrainStationViewModel>>
    {
        private readonly ITrainStationRepository _repository;
        private readonly IMapper _mapper;

        public GetTrainStationByIdHandler(ITrainStationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<TrainStationViewModel>> Handle(GetTrainStationByIdQuery query, CancellationToken cancellationToken)
        {
            var model = await _repository.GetByIdAsync(query.Id);
            if (model == null) throw new ApiException($"Станция не найдена.");
            var resultViewModel = _mapper.Map<TrainStationViewModel>(model);
            return new Response<TrainStationViewModel>(resultViewModel, "Станция найдена.");
        }
    }
}
