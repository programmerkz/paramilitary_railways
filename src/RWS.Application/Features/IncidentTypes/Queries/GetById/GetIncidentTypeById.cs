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

namespace RWS.Application.Features.IncidentTypes.Queries.GetById
{
    public class GetIncidentTypeByIdQuery : IRequest<Response<IncidentTypeViewModel>>
    {
        public Guid Id { get; set; }
    }

    public class GetIncidentTypeByIdHandler : IRequestHandler<GetIncidentTypeByIdQuery, Response<IncidentTypeViewModel>>
    {
        private readonly IIncidentTypeRepository _repository;
        private readonly IMapper _mapper;

        public GetIncidentTypeByIdHandler(IIncidentTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IncidentTypeViewModel>> Handle(GetIncidentTypeByIdQuery query, CancellationToken cancellationToken)
        {
            var model = await _repository.GetByIdAsync(query.Id);
            if (model == null) throw new ApiException("Тип инцидента не найден.");
            var resultViewModel = _mapper.Map<IncidentTypeViewModel>(model);
            return new Response<IncidentTypeViewModel>(resultViewModel, "Тип инцидента найден.");
        }
    }
}
