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

namespace RWS.Application.Features.Incidents.Queries.GetById
{
    public class GetIncidentByIdQuery : IRequest<Response<IncidentViewModel>>
    {
        public Guid Id { get; set; }
    }

    public class GetIncidentByIdHandler : IRequestHandler<GetIncidentByIdQuery, Response<IncidentViewModel>>
    {
        private readonly IIncidentRepository _repository;
        private readonly IMapper _mapper;

        public GetIncidentByIdHandler(IIncidentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IncidentViewModel>> Handle(GetIncidentByIdQuery query, CancellationToken cancellationToken)
        {
            var model = await _repository.GetByIdWithDictsAsync(query.Id);
            if (model == null) throw new ApiException($"Инцидент не найден.");
            var resultViewModel = _mapper.Map<IncidentViewModel>(model);
            return new Response<IncidentViewModel>(resultViewModel, "Инцидент найден.");
        }
    }
}
