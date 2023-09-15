using RWS.Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RWS.Application.Features.Incidents.Commands.Create
{
    public class CreateIncidentCommandValidator : AbstractValidator<CreateIncidentCommand>
    {
        private readonly IIncidentRepository _repository;
        private readonly ITrainStationRepository _trainStationRepository;
        private readonly IIncidentTypeRepository _incidentTypeRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public CreateIncidentCommandValidator
            (IIncidentRepository repository,
            ITrainStationRepository trainStationRepository,
            IIncidentTypeRepository incidentTypeRepository,
            IEmployeeRepository employeeRepository)
        {

            _repository = repository;
            _trainStationRepository = trainStationRepository;
            _incidentTypeRepository = incidentTypeRepository;
            _employeeRepository = employeeRepository;


            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("{PropertyName} является обязательным.")
                .NotNull()
                .MaximumLength(80).WithMessage("{PropertyName} не должно превышать 80 символов.");

            RuleFor(p => p.Text)
                .NotEmpty().WithMessage("{PropertyName} является обязательным.")
                .NotNull()
                .MaximumLength(1000).WithMessage("{PropertyName} не должно превышать 1000 символов.");

            RuleFor(p => p.TrainStationId)
                .MustAsync(TrainStationExists).WithMessage("{PropertyName} не найден.");

            RuleFor(p => p.IncidentTypeId)
                .MustAsync(IncidentTypeExists).WithMessage("{PropertyName} не найден.");

            RuleFor(p => p.EmployeeId)
                .MustAsync(EmployeeExists).WithMessage("{PropertyName} не найден.");
        }

        private async Task<bool> TrainStationExists(Guid id, CancellationToken cancellationToken)
            => await _trainStationRepository.Exists(id);

        private async Task<bool> IncidentTypeExists(Guid id, CancellationToken cancellationToken)
            => await _incidentTypeRepository.Exists(id);

        private async Task<bool> EmployeeExists(Guid id, CancellationToken cancellationToken)
            => await _employeeRepository.Exists(id);
    }
}
