using RWS.Application.Exceptions;
using RWS.Application.Interfaces.Repositories;
using RWS.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using RWS.Application.ViewModels;

namespace RWS.Application.Features.IncidentTypes.Commands.Update
{
    public class UpdateIncidentTypeCommand : IRequest<Response<IncidentTypeViewModel>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateIncidentTypeCommandHandler : IRequestHandler<UpdateIncidentTypeCommand, Response<IncidentTypeViewModel>>
    {
        private readonly IIncidentTypeRepository _repository;
        private readonly IMapper _mapper;

        public UpdateIncidentTypeCommandHandler(IIncidentTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IncidentTypeViewModel>> Handle(UpdateIncidentTypeCommand request, CancellationToken cancellationToken)
        {
            var model = await _repository.GetByIdAsync(request.Id);

            if (model == null)
            {
                throw new ApiException("Тип инцидента не найден.");
            }
            else
            {
                model.Name = request.Name;
                await _repository.UpdateAsync(model);
                var viewModel = _mapper.Map<IncidentTypeViewModel>(model);
                return new Response<IncidentTypeViewModel>(viewModel, "Тип инцидента успешно обновлен.");
            }
        }
    }
}
