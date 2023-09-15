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

namespace RWS.Application.Features.IncidentTypes.Queries.GetAll
{
    public class GetAllIncidentTypesQuery : IRequest<PagedResponse<IEnumerable<IncidentTypeViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllIncidentTypesQueryHandler : IRequestHandler<GetAllIncidentTypesQuery, PagedResponse<IEnumerable<IncidentTypeViewModel>>>
    {
        private readonly IIncidentTypeRepository _repository;
        private readonly IMapper _mapper;

        public GetAllIncidentTypesQueryHandler(IIncidentTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<IncidentTypeViewModel>>> Handle(GetAllIncidentTypesQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllIncidentTypesParameter>(request);
            var result = await _repository.GetAllPagedResponseAsync(validFilter.PageNumber, validFilter.PageSize);
            var itemCount = await _repository.Count();
            var resultViewModel = _mapper.Map<IEnumerable<IncidentTypeViewModel>>(result);
            var message = itemCount == 0 ? "Типы инцидентов не найдены." : "Типы инцидентов найдены.";
            return new PagedResponse<IEnumerable<IncidentTypeViewModel>>(resultViewModel, validFilter.PageNumber, validFilter.PageSize, itemCount, message);
        }
    }
}
