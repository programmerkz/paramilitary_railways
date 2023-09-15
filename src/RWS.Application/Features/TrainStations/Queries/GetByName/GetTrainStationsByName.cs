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
using System.Linq;

namespace RWS.Application.Features.TrainStations.Queries.GetByName
{
    public class GetTrainStationsByNameQuery : IRequest<Response<IReadOnlyList<TrainStationViewModel>>>
    {
        public string SearchingString { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetTrainStationsByNameHandler : IRequestHandler<GetTrainStationsByNameQuery, Response<IReadOnlyList<TrainStationViewModel>>>
    {
        private readonly ITrainStationRepository _repository;
        private readonly IMapper _mapper;

        public GetTrainStationsByNameHandler(ITrainStationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IReadOnlyList<TrainStationViewModel>>> Handle(GetTrainStationsByNameQuery query, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetTrainStationsByNameParameter>(query);

            var model = await _repository.GetByNameFilterAsync(query.SearchingString, validFilter.PageNumber, validFilter.PageSize);
            if (model == null) throw new ApiException($"Станции не найдена.");
            var resultViewModel = _mapper.Map<IReadOnlyList<TrainStationViewModel>>(model);
            return new Response<IReadOnlyList<TrainStationViewModel>>(resultViewModel,
                $"Станции найдены. {model.Count}, {resultViewModel.Count}, {validFilter.PageNumber}, {validFilter.PageSize}");
        }
    }
}
