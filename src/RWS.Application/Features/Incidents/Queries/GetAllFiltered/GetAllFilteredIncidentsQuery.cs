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

namespace RWS.Application.Features.Incidents.Queries.GetAllFiltered
{
    public class GetAllFilteredIncidentsQuery : IRequest<PagedResponse<IEnumerable<IncidentViewModel>>>
    {
        public bool IsSent { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllFilteredIncidentsQueryHandler : IRequestHandler<GetAllFilteredIncidentsQuery, PagedResponse<IEnumerable<IncidentViewModel>>>
    {
        private readonly IIncidentRepository _repository;
        private readonly IMapper _mapper;

        public GetAllFilteredIncidentsQueryHandler(IIncidentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<IncidentViewModel>>> Handle(GetAllFilteredIncidentsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllFilteredIncidentsParameter>(request);
            var result = await _repository.GetAllWithDictsPagedResponseAsync(validFilter.PageNumber, validFilter.PageSize, validFilter.IsSent);
            var itemCount = await _repository.Count(validFilter.IsSent);
            var resultViewModel = _mapper.Map<IEnumerable<IncidentViewModel>>(result);
            var message = itemCount == 0 ? "Инциденты не найдены." : "Инциденты найдены.";
            return new PagedResponse<IEnumerable<IncidentViewModel>>(resultViewModel, validFilter.PageNumber, validFilter.PageSize, itemCount, message);
        }
    }
}
