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

namespace RWS.Application.Features.IncidentTypes.Commands.Create
{
    public class CreateIncidentTypeCommand : IRequest<Response<IncidentTypeViewModel>>
    {
        public string Name { get; set; }
    }

    public class CreateIncidentTypeCommandHandler : IRequestHandler<CreateIncidentTypeCommand, Response<IncidentTypeViewModel>>
    {
        private readonly IIncidentTypeRepository _repository;
        private readonly IMapper _mapper;

        public CreateIncidentTypeCommandHandler(IIncidentTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IncidentTypeViewModel>> Handle(CreateIncidentTypeCommand command, CancellationToken cancellationToken)
        {
            var model = _mapper.Map<IncidentType>(command);
            await _repository.AddAsync(model);
            var viewModel = _mapper.Map<IncidentTypeViewModel>(model);
            return new Response<IncidentTypeViewModel>(viewModel, "Тип инцидента успешно добавлен.");
        }
    }
}
