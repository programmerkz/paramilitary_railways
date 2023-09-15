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

namespace RWS.Application.Features.Incidents.Commands.Create
{
    public class CreateIncidentCommand : IRequest<Response<IncidentViewModel>>
    {
        public string Title { get; set; }
        public string Text { get; set; }

        public Guid TrainStationId { get; set; }
        public Guid IncidentTypeId { get; set; }
        public Guid EmployeeId { get; set; }
    }

    public class CreateIncidentCommandHandler : IRequestHandler<CreateIncidentCommand, Response<IncidentViewModel>>
    {
        private readonly IIncidentRepository _repository;
        private readonly IMapper _mapper;

        public CreateIncidentCommandHandler(IIncidentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IncidentViewModel>> Handle(CreateIncidentCommand command, CancellationToken cancellationToken)
        {
            var model = _mapper.Map<Incident>(command);
            await _repository.AddAsync(model);
            var viewModel = _mapper.Map<IncidentViewModel>(model);
            return new Response<IncidentViewModel>(viewModel, "Инцидент успешно создан.");
        }
    }
}
