using RWS.Application.Exceptions;
using RWS.Application.Interfaces.Repositories;
using RWS.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RWS.Application.ViewModels;
using AutoMapper;

namespace RWS.Application.Features.Incidents.Commands.Update
{
    public class UpdateIncidentCommand : IRequest<Response<IncidentViewModel>>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        public Guid TrainStationId { get; set; }
        public Guid IncidentTypeId { get; set; }
        public Guid EmployeeId { get; set; }
    }

    public class UpdateIncidentCommandHandler : IRequestHandler<UpdateIncidentCommand, Response<IncidentViewModel>>
    {
        private readonly IIncidentRepository _repository;
        private readonly IMapper _mapper;

        public UpdateIncidentCommandHandler(IIncidentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IncidentViewModel>> Handle(UpdateIncidentCommand request, CancellationToken cancellationToken)
        {
            var model = await _repository.GetByIdAsync(request.Id);

            if (model == null)
            {
                throw new ApiException($"Инцидент не найден.");
            }
            else
            {
                model.Title = request.Title;
                model.Text = request.Text;
                model.TrainStationId = request.TrainStationId;
                model.IncidentTypeId = request.IncidentTypeId;
                model.EmployeeId = request.EmployeeId;

                await _repository.UpdateAsync(model);
                var viewModel = _mapper.Map<IncidentViewModel>(model);

                return new Response<IncidentViewModel>(viewModel, "Инцидент успешно обновлен.");
            }
        }
    }
}
