using AutoMapper;
using RWS.Application.Interfaces.Repositories;
using RWS.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RWS.Application.ViewModels;

namespace RWS.Application.Features.TrainStations.Queries.GetAll
{
    public class GetAllTrainStationsQuery : IRequest<PagedResponse<IEnumerable<TrainStationViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllTrainStationsQueryHandler : IRequestHandler<GetAllTrainStationsQuery, PagedResponse<IEnumerable<TrainStationViewModel>>>
    {
        private readonly ITrainStationRepository _repository;
        private readonly IMapper _mapper;

        public GetAllTrainStationsQueryHandler(ITrainStationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<TrainStationViewModel>>> Handle(GetAllTrainStationsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllTrainStationsParameter>(request);
            var result = await _repository.GetAllPagedResponseAsync(validFilter.PageNumber, validFilter.PageSize);
            var itemCount = await _repository.Count();
            var resultViewModel = _mapper.Map<IEnumerable<TrainStationViewModel>>(result);
            var message = itemCount == 0 ? "Станции не найдены." : "Станции найдены.";
            return new PagedResponse<IEnumerable<TrainStationViewModel>>(resultViewModel, validFilter.PageNumber, validFilter.PageSize, itemCount, message);
        }
    }
}
