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
using Microsoft.AspNetCore.Mvc;
using RWS.Application.ModelBindings;

namespace RWS.Application.Features.Incidents.Queries.GetAll
{
    public class GetAllIncidentsQuery : IRequest<PagedResponse<IEnumerable<IncidentViewModel>>>
    {
        public string Name { get; set; }
        public bool IsSend  { get; set; }
        [ModelBinder(typeof(CommaDelimitedArrayModelBinder))]
        public ICollection<Guid> IncidentTypeIds { get; set; } = new List<Guid>();
        [ModelBinder(typeof(CommaDelimitedArrayModelBinder))]
        public ICollection<Guid> TrainStationIds { get; set; } = new List<Guid>();
        [ModelBinder(typeof(CommaDelimitedArrayModelBinder))]
        public ICollection<Guid> IndcidentIds { get; set; } = new List<Guid>();
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllIncidentsQueryHandler : IRequestHandler<GetAllIncidentsQuery, PagedResponse<IEnumerable<IncidentViewModel>>>
    {
        private readonly IIncidentRepository _repository;
        private readonly IMapper _mapper;

        public GetAllIncidentsQueryHandler(IIncidentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<IncidentViewModel>>> Handle(GetAllIncidentsQuery request, CancellationToken cancellationToken)
        {
            var itemCount = await _repository.Count(request);

            if (request.PageNumber == 0 && request.PageSize == 0)
            {
                request.PageNumber = 1;
                request.PageSize = itemCount;
            }

            var result = await _repository.GetAllAsync(request);
            var resultViewModel = _mapper.Map<IEnumerable<IncidentViewModel>>(result);
            var message = itemCount == 0 ? "Инциденты не найдены." : "Инциденты найдены.";
            return new PagedResponse<IEnumerable<IncidentViewModel>>(resultViewModel, request.PageNumber, request.PageSize, itemCount, message);
        }
    }
}
