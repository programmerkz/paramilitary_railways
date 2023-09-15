using RWS.Application.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RWS.Application.Features.TrainStations.Commands.Create
{
    public class CreateTrainStationCommandValidator : AbstractValidator<CreateTrainStationCommand>
    {
        private readonly ITrainStationRepository _repository;

        public CreateTrainStationCommandValidator(ITrainStationRepository repository)
        {
            _repository = repository;

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} является обязательным.")
                .NotNull()
                .MaximumLength(80).WithMessage("{PropertyName} не должно превышать 80 символов.");

            //RuleFor(p => p.RegionId)
            //    .MustAsync(IsRegionExists).WithMessage("{PropertyName} не найден.");
        }

        //private Task<bool> IsRegionExists(Guid id, CancellationToken cancellationToken)
        //    => _regions.Exists(id);
    }
}
